using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMessageLogger.DataAccess;
using MvcMessageLogger.Models;

namespace MvcMessageLogger.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly MvcMessageLoggerContext _context;

        public StatisticsController(MvcMessageLoggerContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            var wordsList = new List<string>();
            var wordsList2 = new List<string>();
            var messages = _context.Messages.ToList();
            Statistics stat = new Statistics { statModel = "statModel"};

            foreach (var message in messages)
            {
                wordsList2.AddRange(message.Content.ToLower().Split());
            }

            foreach (var user in users)
            {
                foreach (var m in user.Messages)
                {
                    wordsList.AddRange(m.Content.ToLower().Split());
                }
            }
            ViewData["MostCommonPublic"] = stat.MostCommonPublic(wordsList2);
            ViewData["ActiveHour"] = stat.ActiveHour(_context);
            ViewData["MessageCountDesc"] = stat.MessageCountDescUser(_context);
            return View(users);
        }

        [HttpPost]
        [Route("/statistics/select")]
        public IActionResult Select(string UserSelect)
        {
            Statistics stat = new Statistics();
            var wordsList = new List<String>();
            //var user2 = _context.Users.Where(e => e.Username == UserSelect).Single();//SELECTS USER!
            var user = _context.Users.Include(e => e.Messages).Where(e => e.Username == UserSelect).Single();
            //create view data for most common words list, this will also allow the use of a @if(ViewData != null) in View
            //then display.

            //var userMessages = user.Messages.ToList();
            //foreach (var e in userMessages)
            //{
            //    wordsList.AddRange(e.Content.ToLower().Split());
            //}

            //foreach (var e in wordsList)
            //{
            //    Console.WriteLine(e);//this does print: words are getting split properly.
            //}

            //ViewData["SingleUserCommonWord"] = stat.MostCommonPublic(wordsList);

            ViewData["MostCommonPersonal"] = stat.MostCommonPersonal(_context, user);


            return View();
        }
    }
}
