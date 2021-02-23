using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbBicyclesLab.Models;

namespace DbBicyclesLab.Controllers
{
    public class SizeColorModelsController : Controller
    {
        private readonly DBBicyclesContext _context;

        public SizeColorModelsController(DBBicyclesContext context)
        {
            _context = context;
        }

        // GET: SizeColorModels
        public async Task<IActionResult> Index()
        {
            var dBBicyclesContext = _context.SizeColorModels.Include(s => s.Color).Include(s => s.Model).Include(s => s.Size);
            return View(await dBBicyclesContext.ToListAsync());
        }

        // GET: SizeColorModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sizeColorModel = await _context.SizeColorModels
                .Include(s => s.Color)
                .Include(s => s.Model)
                .Include(s => s.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sizeColorModel == null)
            {
                return NotFound();
            }

            return View(sizeColorModel);
        }

        // GET: SizeColorModels/Create
        public IActionResult Create()
        {
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName");
            ViewData["ModelId"] = new SelectList(_context.BicycleModels, "Id", "ModelName");
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "SizeName");
            return View();
        }

        public async Task<IActionResult> CreateBicycle(int? id)
        {
            RedirectToActionResult redirectToActionResult = RedirectToAction("Create", "Bicycles", new { sizeColorModelId = id });
            return await Task.Run<IActionResult>(() =>
            {
                return redirectToActionResult;
            });
        }

        // POST: SizeColorModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SizeId,ColorId,ModelId")] SizeColorModel sizeColorModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sizeColorModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName", sizeColorModel.ColorId);
            ViewData["ModelId"] = new SelectList(_context.BicycleModels, "Id", "ModelName", sizeColorModel.ModelId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "SizeName", sizeColorModel.SizeId);
            return View(sizeColorModel);
        }

        // GET: SizeColorModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sizeColorModel = await _context.SizeColorModels.FindAsync(id);
            if (sizeColorModel == null)
            {
                return NotFound();
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName", sizeColorModel.ColorId);
            ViewData["ModelId"] = new SelectList(_context.BicycleModels, "Id", "ModelName", sizeColorModel.ModelId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "SizeName", sizeColorModel.SizeId);
            return View(sizeColorModel);
        }

        // POST: SizeColorModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SizeId,ColorId,ModelId")] SizeColorModel sizeColorModel)
        {
            if (id != sizeColorModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sizeColorModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeColorModelExists(sizeColorModel.Id))
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
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName", sizeColorModel.ColorId);
            ViewData["ModelId"] = new SelectList(_context.BicycleModels, "Id", "ModelName", sizeColorModel.ModelId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "SizeName", sizeColorModel.SizeId);
            return View(sizeColorModel);
        }

        // GET: SizeColorModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sizeColorModel = await _context.SizeColorModels
                .Include(s => s.Color)
                .Include(s => s.Model)
                .Include(s => s.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sizeColorModel == null)
            {
                return NotFound();
            }

            return View(sizeColorModel);
        }

        // POST: SizeColorModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sizeColorModel = await _context.SizeColorModels.FindAsync(id);
            _context.SizeColorModels.Remove(sizeColorModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SizeColorModelExists(int id)
        {
            return _context.SizeColorModels.Any(e => e.Id == id);
        }
    }
}
