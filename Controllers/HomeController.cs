using FirstCoreApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Linq;

namespace FirstCoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly NewsContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeController(NewsContext context, ILogger<HomeController> logger)
        {
            _db = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var result = _db.Categories.ToList();
            return View(result);

            if (User.Identity.IsAuthenticated)
            {
                HttpContext.Session.SetString("userEmail", User.Identity.Name.ToString());
            }
            else
            {
                HttpContext.Session.SetString("userEmail", "");
            }
        }

        public IActionResult Messages()
        {
            var x = HttpContext.Session.GetString("userEmail");
            return View(_db.Contacts.ToList());
        }
        public IActionResult Teammembers()
        {
            return View(_db.Teammembers.ToList());
        }

        [Authorize]
        public IActionResult News(int id)
        {
            Category c = _db.Categories.Find(id);
            ViewBag.cat = c.Name;
            ViewData["Cat"] = c.Name;

            var result = _db.News.Where(x => x.CategoryId == id).OrderByDescending(x => x.Date).ToList();
            return View(result);
        }

        public IActionResult DeleteNews(int id)
        {
            var news = _db.News.Find(id);
            _db.News.Remove(news);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveContact(ContactUs model)
        {
            if (ModelState.IsValid)
            {
                _db.Contacts.Add(model);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Contact", model); // إعادة عرض نموذج الاتصال مع الأخطاء إن وجدت
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
