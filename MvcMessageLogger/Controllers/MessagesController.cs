using Microsoft.AspNetCore.Mvc;
using MvcMessageLogger.DataAccess;
using MvcMessageLogger.Models;

namespace MvcMessageLogger.Controllers
{
    public class MessagesController : Controller
    {
        private readonly MvcMessageLoggerContext _context;

        public MessagesController(MvcMessageLoggerContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("/users/details/{id:int}/message")]
        public IActionResult Index(int id, Message message)
        {
            var user = _context.Users.Find(id);
            Message message2 = new Message{ Content = message.Content, CreatedAt = DateTime.Now.ToUniversalTime()};

            user.Messages.Add(message2);
            _context.SaveChanges();

            return Redirect($"/users/details/{user.Id}");
        }

        ///{messageId:int}
        [Route("/messages/edit/{id:int}")]
        public IActionResult Edit(int id)
        {
            var message = _context.Messages.Find(id);

            return View(message);
        }

        [HttpPost]
        [Route("/messages/{id:int}")]
        public IActionResult Update(int id, Message message)
        {
            message.Id = id;
            _context.Update(message);
            _context.SaveChanges();

            var user = _context.Users.Where(e => e.Messages.Contains(message)).ToList().Single();

            return Redirect($"/users/details/{user.Id}");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var message = _context.Messages.Find(id);
            var user = _context.Users.Where(e => e.Messages.Contains(message)).ToList().Single();
            _context.Remove(message);
            _context.SaveChanges();
            return Redirect($"/users/details/{user.Id}");
        }
    }
}
