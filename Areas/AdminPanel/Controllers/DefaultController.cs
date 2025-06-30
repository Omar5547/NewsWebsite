using FirstCoreApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstCoreApp.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class DefaultController : Controller
    {
        private readonly NewsContext db;

        public DefaultController(NewsContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            int teamCount = db.Teammembers.Count();
            int newsCount = db.News.Count();
            int contactCount = db.Contacts.Count();
            int catCount = db.Categories.Count();

            return View(new AdminNumbers { Team = teamCount, News = newsCount, Contact = contactCount, Cats = catCount });
        }
    }
}
