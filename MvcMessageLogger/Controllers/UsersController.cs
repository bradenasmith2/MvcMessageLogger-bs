using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMessageLogger.DataAccess;
using MvcMessageLogger.Models;

namespace MvcMessageLogger.Controllers
{
    public class UsersController : Controller
    {
        private readonly MvcMessageLoggerContext _context;

        public UsersController(MvcMessageLoggerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["random"] = TempData["random"];
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Details(int id)
        {
            var u = _context.Users.Include(e => e.Messages).Where(e => e.Id == id).Single();
            return View(u);
        }

        public IActionResult New()
        {
            ViewData["UsernameTaken"] = TempData["UsernameTaken"];
            return View();
        }

        [HttpPost]//CREATE
        public IActionResult Index(User user)
        {
            
            if (_context.Users.Any(e => e.Username == user.Username))
            {
                TempData["UsernameTaken"] = "That username is already taken, please enter another.";//TempData can be used to transfer data from one controller to the next
                return RedirectToAction("New");
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return Redirect($"/users/details/{user.Id}");
        }

        public IActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        [Route("/users/Details/{id:int}")]
        public IActionResult Update(int id, User user)
        {
            user.Id = id;
            _context.Users.Update(user);
            _context.SaveChanges();

            return Redirect($"/users/details/{user.Id}");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Include(e => e.Messages).Where(e => e.Id == id).Single();
            _context.Users.Remove(user);
            _context.SaveChanges();

            return Redirect("/users");
        }

        [Route("/users/login")]
        public IActionResult LogInForm()
        {
            ViewData["IncorrectLogin"] = TempData["IncorrectLogin"];
            return View();
        }

        [HttpPost]
        [Route("/users/login/attempt")]
        public IActionResult LogInResult(string Username, string Password)
        {
            if (_context.Users.Any(e => e.Username == Username && _context.Users.Any(e => e.Password == Password)))
            {
                var user = _context.Users.Where(e => e.Username == Username).Single();

                return Redirect($"/users/details/{user.Id}");
            }
            else if(_context.Users.Any(e => e.Username != Username))
            {
                TempData["IncorrectLogin"] = "Sorry but either the password or username is incorrect, please try again.";
            }
            return Redirect("/users/login");
        }

        public IActionResult Generate()
        {
            Random rdm = new Random();
            var messages = _context.Messages.ToList();
            TempData["random"] = messages[rdm.Next(messages.Count)].Content;
            return Redirect("/users");
        }
    }
}

/*
 USER LOG IN LOGIC THOUGHTS

    A button appears on the User Index page that says "Login" instead of a button per user.

    The button would send a route to an action, labeled "login"
        "login" would return a form for the user to enter their username, and **password**
            Then they would be "rerouted" to another action labeled "loginLogic"
                "loginLogic" would then verify that the username submitted by the User DOES exist in the database, IF it does, then it would check if the password is correct.
                    If one or the other is wrong: "sorry, your password or username is incorrect"
                    
                    If successful, show Details

        If username == 'context.usernames'
        user = .Find(username)
            If password == user.password
            
        Log in

        Else Incorrect 
 */