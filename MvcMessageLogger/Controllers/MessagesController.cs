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
    }
}
