using FirstCoreApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstCoreApp.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class mangenewsController : Controller
    {
        private readonly NewsContext _context;

        public mangenewsController(NewsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult getNews()
        {
            return Ok(_context.News.ToList());
        }
    }
}
