using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbBicyclesLab.Models;

using System.ComponentModel.DataAnnotations;

namespace DbBicyclesLab.Controllers
{
    public class BicycleModelsController : Controller
    {
        private readonly DBBicyclesContext _context;

        public BicycleModelsController(DBBicyclesContext context)
        {
            _context = context;
        }

        // GET: BicycleModels
        public async Task<IActionResult> Index()
        {
            var dBBicyclesContext = _context.BicycleModels.Include(b => b.Brand).Include(b => b.Category).Include(b => b.Gender);
            return View(await dBBicyclesContext.ToListAsync());
        }

        // GET: BicycleModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bicycleModel = await _context.BicycleModels
                .Include(b => b.Brand)
                .Include(b => b.Category)
                .Include(b => b.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bicycleModel == null)
            {
                return NotFound();
            }

            return View(bicycleModel);
        }

        // GET: BicycleModels/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "BrandName");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "GenderName");
            return View();
        }

        // POST: BicycleModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,ModelName,ModelYear,BrandId,GenderId,CategoryId,Description")] BicycleModel bicycleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bicycleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "BrandName", bicycleModel.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", bicycleModel.CategoryId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "GenderName", bicycleModel.GenderId);
            return View(bicycleModel);
        }

        // GET: BicycleModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bicycleModel = await _context.BicycleModels.FindAsync(id);
            if (bicycleModel == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "BrandName", bicycleModel.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", bicycleModel.CategoryId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "GenderName", bicycleModel.GenderId);
            return View(bicycleModel);
        }

        // POST: BicycleModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,ModelName,ModelYear,BrandId,GenderId,CategoryId,Description")] BicycleModel bicycleModel)
        {
            if (id != bicycleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bicycleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BicycleModelExists(bicycleModel.Id))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "BrandName", bicycleModel.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", bicycleModel.CategoryId);
            ViewData["GenderId"] = new SelectList(_context.Genders, "Id", "GenderName", bicycleModel.GenderId);
            return View(bicycleModel);
        }

        // GET: BicycleModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bicycleModel = await _context.BicycleModels
                .Include(b => b.Brand)
                .Include(b => b.Category)
                .Include(b => b.Gender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bicycleModel == null)
            {
                return NotFound();
            }

            return View(bicycleModel);
        }

        // POST: BicycleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bicycleModel = await _context.BicycleModels.FindAsync(id);
            _context.BicycleModels.Remove(bicycleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BicycleModelExists(int id)
        {
            return _context.BicycleModels.Any(e => e.Id == id);
        }
    }
}
