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
            return View();
        }

        [HttpPost]//CREATE
        public IActionResult Index(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return Redirect($"/users/details/{user.Id}");
        }





        [Route("/users/login")]
        public IActionResult LogInForm()
        {
            return View();
        }

        //[HttpPost]
        //[Route("/users/login/check")]
        //public IActionResult LogInLogic(string name, string username)
        //{
            //var user = _context.Users.Where(e => e.Name == name).Single();

            //if (user.Name == name)
            //{
                //if (user.Username == username)
                //{
                   // return Redirect($"/users/{user.Id}");
                //}
            //}
        //}
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