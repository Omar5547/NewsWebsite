using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstCoreApp.Models;
using Microsoft.Extensions.Hosting;

namespace FirstCoreApp.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class TeammembersController : Controller
    {
        private readonly NewsContext _context;
        private readonly IWebHostEnvironment host; // Add this line to get the host environment

        public TeammembersController(NewsContext context, IWebHostEnvironment host) // Add host parameter to constructor
        {
            _context = context;
            this.host = host; // Assign host to the field
        }

        // GET: Teammembers
        public async Task<IActionResult> Index()
        {
            return _context.Teammembers != null ?
                        View(await _context.Teammembers.ToListAsync()) :
                        Problem("Entity set 'NewsContext.Teammembers'  is null.");
        }

        // GET: Teammembers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teammembers == null)
            {
                return NotFound();
            }

            var teammember = await _context.Teammembers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teammember == null)
            {
                return NotFound();
            }

            return View(teammember);
        }

        // GET: Teammembers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teammembers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teammember teammember)
        {
            if (ModelState.IsValid)
            {
                if (teammember.imageFile != null && teammember.imageFile.Length > 0)
                {
                    // معالجة وتحميل الملف
                    string uploadsFolder = Path.Combine(host.WebRootPath, "Images");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + teammember.imageFile.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await teammember.imageFile.CopyToAsync(fileStream);
                    }
                    teammember.Image = uniqueFileName;
                }
                else
                {
                    ModelState.AddModelError("imageFile", "The Image field is required.");
                }

                if (ModelState.IsValid)
                {
                    _context.Add(teammember);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(teammember);
        }









        // GET: Teammembers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teammembers == null)
            {
                return NotFound();
            }

            var teammember = await _context.Teammembers.FindAsync(id);
            if (teammember == null)
            {
                return NotFound();
            }
            return View(teammember);
        }

        // POST: Teammembers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Teammember teammember)
        {


            if (ModelState.IsValid)
            {
                try
                {
                    uploadphoto(teammember);
                    _context.Update(teammember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeammemberExists(teammember.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(teammember);
        }

        // GET: Teammembers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teammembers == null)
            {
                return NotFound();
            }

            var teammember = await _context.Teammembers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teammember == null)
            {
                return NotFound();
            }

            return View(teammember);
        }

        // POST: Teammembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teammembers == null)
            {
                return Problem("Entity set 'NewsContext.Teammembers'  is null.");
            }
            var teammember = await _context.Teammembers.FindAsync(id);
            if (teammember != null)
            {
                _context.Teammembers.Remove(teammember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeammemberExists(int id)
        {
            return (_context.Teammembers?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        // Add the uploadphoto method here
        void uploadphoto(Teammember model)
        {
            if (model.imageFile != null)
            {
                string uploadsFolder = Path.Combine(host.WebRootPath, "~/AdminPanel/images/News");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.imageFile.CopyTo(fileStream);
                }
                model.Image = uniqueFileName;
            }
        }
    }
}
