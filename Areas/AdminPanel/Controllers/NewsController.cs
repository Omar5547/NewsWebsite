using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstCoreApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace FirstCoreApp.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class NewsController : Controller
    {
        private readonly NewsContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<NewsController> _logger;

        public NewsController(NewsContext context, IWebHostEnvironment host, ILogger<NewsController> logger)
        {
            _context = context;
            _hostEnvironment = host;
            _logger = logger;
        }

        //https://localhost:7112/AdminPanel/News/getAllNews
        public IActionResult getAllNews()
        {
            try
            {
                var newsData = _context.News.Select(x => new { x.Title, x.Date, x.Category }).ToList();
                return Ok(newsData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching news data.");
                return StatusCode(500, "Internal server error");
            }
        }


        // GET: AdminPanel/News/Charts
        public IActionResult Charts()
        {
            return View();
        }

        // باقي الأكشنات
        // GET: News
        public async Task<IActionResult> Index()
        {
            var newsContext = _context.News.Include(n => n.Category);
            return View(await newsContext.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News news)
        {
            _logger.LogInformation("Entered Create POST method");

            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model state is valid");

                try
                {
                    if (news.file != null)
                    {
                        _logger.LogInformation("File is not null");

                        string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "~/AdminPanel/images/News");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                            _logger.LogInformation("Created uploads folder");
                        }

                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(news.file.FileName);
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await news.file.CopyToAsync(fileStream);
                            _logger.LogInformation("File copied to the server");
                        }

                        news.Image = uniqueFileName;
                    }

                    _context.Add(news);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("News item saved successfully");

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating news item");
                    ModelState.AddModelError(string.Empty, "An error occurred while uploading the image. Please try again.");
                }
            }
            else
            {
                _logger.LogWarning("Model state is not valid");
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, News news)
        {
            if (id != news.Id)
            {
                _logger.LogWarning("News item id mismatch");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(news);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("تم تحديث الخبر بنجاح");
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsExists(news.Id))
                    {
                        _logger.LogWarning("لم يتم العثور على الخبر بالمعرف: {Id}", news.Id);
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("حدث خطأ أثناء تحديث الخبر بالمعرف: {Id}", news.Id);
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error saving news item");
                    ModelState.AddModelError(string.Empty, "حدث خطأ أثناء تحديث الخبر. يرجى المحاولة مرة أخرى.");
                }
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", news.CategoryId);
            return View(news);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.News == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(n => n.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.News == null)
            {
                _logger.LogError("News context is null");
                return Problem("Entity set 'NewsContext.News' is null.");
            }

            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _logger.LogInformation("News item found and deleted");
                _context.News.Remove(news);
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("News item with id {Id} not found for deletion", id);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return (_context.News?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void uploadphoto(News model)
        {
            if (model.file != null)
            {
                _logger.LogInformation("Starting photo upload");
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "~/AdminPanel/images/News");
                string uniqueFileName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.file.CopyTo(fileStream);
                }
                model.Image = uniqueFileName;
                _logger.LogInformation("Photo uploaded successfully");
            }
            else
            {
                _logger.LogWarning("No file provided for upload");
            }
        }
    }
}
