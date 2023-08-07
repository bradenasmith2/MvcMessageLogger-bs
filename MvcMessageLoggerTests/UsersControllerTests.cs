using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using MvcMessageLogger.Models;
using MvcMessageLogger.DataAccess;
using MvcMessageLogger.FeatureTests;
using System.Runtime.CompilerServices;

namespace MvcMessageLoggerTests
{
    [Collection("Users Controller Tests")]
    public class MvcMessageLoggerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public MvcMessageLoggerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        private MvcMessageLoggerContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MvcMessageLoggerContext>();
            optionsBuilder.UseInMemoryDatabase("TestDatabase");

            var context = new MvcMessageLoggerContext(optionsBuilder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task Index_ReturnsView()
        {
            var context = GetDbContext();

            context.Users.Add(new User { Name = "John Doe", Username = "jdoe123" });
            context.Users.Add(new User { Name = "Jane Doe", Username = "jdoe456" });
            context.SaveChanges();

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/users");
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Contains("jdoe123", html);
            Assert.Contains("jdoe456", html);
            Assert.DoesNotContain("John Doe", html);
        }

        [Fact]
        public async Task New_ReturnsForm()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/users/new");
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Contains("<form method=\"post\" action=\"/users\">", html);
            Assert.Contains("<button type=\"submit\">Create Account</button>", html);
            Assert.Contains("Name", html);
            Assert.Contains("Username", html);
        }

        [Fact]
        public async Task IndexPost_CreatesUserInDb()
        {
            var client = _factory.CreateClient();

            var addItemFormData = new Dictionary<string, string>
            {
                {"Name", "John Doe" },
                {"Username", "jdoe123" }
            };

            var response = await client.PostAsync("/users", new FormUrlEncodedContent(addItemFormData));
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            Assert.Contains("jdoe123", html);
        }

        [Fact]
        public async Task Details_ReturnsViewWithUserAndMessages()
        {
            var context = GetDbContext();

            context.Users.Add(new User { Name = "John Doe", Username = "jdoe123" });
            context.Users.Add(new User { Name = "Jane Doe", Username = "jdoe456" });
            context.SaveChanges();

            var client = _factory.CreateClient();

            var response = await client.GetAsync("/users/details/2");
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Contains("jdoe456", html);
            Assert.Contains("Jane Doe", html);
            Assert.DoesNotContain("jdoe123", html);
            Assert.DoesNotContain("John Doe", html);
        }

        [Fact]
        public async Task MessageIndexPost_CreatesMessageForDetailedUser()
        {
            var client = _factory.CreateClient();
            var context = GetDbContext();

            context.Users.Add(new User { Name = "John Doe", Username = "jdoe123" });
            context.SaveChanges();
            var addItemFormData = new Dictionary<string, string>
            {
                {"Content", "this is a message" },
            };

            var response = await client.PostAsync($"/users/details/1/message", new FormUrlEncodedContent(addItemFormData));
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            Assert.Contains("this is a message", html);
            Assert.Contains("<form method=\"post\" action=\"/users/details/1/message\">", html);
            Assert.Contains("<button type=\"submit\">Add Message</button>", html);
        }
    }
}