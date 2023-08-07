using Microsoft.AspNetCore.Mvc;
using MvcMessageLogger.DataAccess;

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
            return View();
        }
    }
}
