using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using DbBicyclesLab.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

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
        public async Task<IActionResult> Index(int? brand, int? category, int? gender, int? minPrice, int? maxPrice, string year)
        {
            /*var dBBicyclesContext = _context.BicycleModels.Include(b => b.Brand).Include(b => b.Category).Include(b => b.Gender);
            if (id != null)
                return View(await dBBicyclesContext.Where(m => m.CategoryId == id).ToListAsync());
            else
                return View(await dBBicyclesContext.ToListAsync());*/
            BicycleModelsListModelView listModelView = new BicycleModelsListModelView(_context);
            if (brand != null)
            {
                listModelView.BicycleModels = listModelView.BicycleModels.Where(b => b.BrandId == brand);
            }
            if (category != null)
            {
                listModelView.BicycleModels = listModelView.BicycleModels.Where(b => b.CategoryId == category);
            }
            if (gender != null)
            {
                listModelView.BicycleModels = listModelView.BicycleModels.Where(b => b.GenderId == gender);
            }
            if (year != null && year != "Усі")
            {
                listModelView.BicycleModels = listModelView.BicycleModels.Where(b => b.ModelYear == int.Parse(year));
            }
            if (maxPrice != null && minPrice != null)
                listModelView.BicycleModels = listModelView.BicycleModels.Where(b => b.Price >= (minPrice) && b.Price <= (maxPrice));
            return View(listModelView);
        }

        public async Task<IActionResult> CommonIndex(int? id)
        {
            var dBBicyclesContext = _context.BicycleModels.Include(b => b.Brand).Include(b => b.Category).Include(b => b.Gender);
            if (id != null)
                return View(await dBBicyclesContext.Where(m => m.CategoryId == id).ToListAsync());
            else
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

        // GET: BicycleModels/Details/5
        public async Task<IActionResult> CommonDetails(int? id)
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
        /*[HttpPost]
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
        }*/

        [HttpPost]
        public IActionResult Create(BicycleModelViewModel bvm)
        {
            BicycleModel bicycleModel = new BicycleModel
            {
                BrandId = bvm.BrandId,
                CategoryId = bvm.CategoryId,
                GenderId = bvm.GenderId,

                Description = bvm.Description,
                Id = bvm.Id,
                ModelName = bvm.ModelName,
                ModelYear = bvm.ModelYear,
                Price = bvm.Price,
                SizeColorModels = bvm.SizeColorModels
            };

            if (ModelState.IsValid)
            {
                if (bvm.Image != null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(bvm.Image.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)bvm.Image.Length);
                    }
                    bicycleModel.Image = imageData;
                }
                _context.Add(bicycleModel);
                _context.SaveChanges();
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
        public async Task<IActionResult> Edit(int id, BicycleModelViewModel bicycleModel)//[Bind("Id,Price,ModelName,ModelYear,BrandId,GenderId,CategoryId,Description")] BicycleModel bicycleModel)
        {
            if (id != bicycleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tempBicycle = await _context.BicycleModels.Where(b => b.Id == bicycleModel.Id).FirstOrDefaultAsync();

                    tempBicycle.BrandId = bicycleModel.BrandId;
                    tempBicycle.CategoryId = bicycleModel.CategoryId;
                    tempBicycle.GenderId = bicycleModel.GenderId;
                    tempBicycle.Description = bicycleModel.Description;
                    tempBicycle.Id = bicycleModel.Id;
                    tempBicycle.ModelName = bicycleModel.ModelName;
                    tempBicycle.ModelYear = bicycleModel.ModelYear;
                    tempBicycle.Price = bicycleModel.Price;
                    tempBicycle.SizeColorModels = bicycleModel.SizeColorModels;

                    if (bicycleModel.Image != null)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(bicycleModel.Image.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)bicycleModel.Image.Length);
                        }
                        tempBicycle.Image = imageData;
                    }
                    _context.Update(tempBicycle);
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

        public async Task<IActionResult> GoToBicycles(int? id)
        {
            RedirectToActionResult redirectToActionResult = RedirectToAction("Index", "Bicycles", new { model = id });
            return await Task.Run<IActionResult>(() =>
            {
                return redirectToActionResult;
            });
        }
    }
}
