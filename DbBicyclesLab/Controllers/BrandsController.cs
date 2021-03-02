using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbBicyclesLab.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace DbBicyclesLab.Controllers
{
    public class BrandsController : Controller
    {
        private readonly DBBicyclesContext _context;

        public BrandsController(DBBicyclesContext context)
        {
            _context = context;
        }

        // GET: Brands
        public async Task<IActionResult> Index()
        {
            var dBBicyclesContext = _context.Brands.Include(b => b.Country).Include(b => b.Dealer);
            return View(await dBBicyclesContext.ToListAsync());
        }

        public async Task<IActionResult> CommonIndex()
        {
            var dBBicyclesContext = _context.Brands;//.Include(b => b.BrandName).Include(b => b.Image);
            return View(await dBBicyclesContext.ToListAsync());
        }

        // GET: Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .Include(b => b.Country)
                .Include(b => b.Dealer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        public async Task<IActionResult> CommonDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .Include(b => b.Country)
                .Include(b => b.Dealer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Brands/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName");
            ViewData["DealerId"] = new SelectList(_context.AuthorizedDealers, "Id", "DealerName");
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BrandName,CountryId,DealerId,Description")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", brand.CountryId);
            ViewData["DealerId"] = new SelectList(_context.AuthorizedDealers, "Id", "DealerName", brand.DealerId);
            return View(brand);
        }*/

        [HttpPost]
        public IActionResult Create(BrandViewModel bvm)
        {
            Brand person = new Brand { BrandName = bvm.BrandName, BicycleModels = bvm.BicycleModels, Country = bvm.Country,
                                        CountryId = bvm.CountryId, Dealer = bvm.Dealer, DealerId = bvm.DealerId,
                                            Description = bvm.Description};
            if (bvm.Image != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(bvm.Image.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)bvm.Image.Length);
                }
                person.Image = imageData;
            }
            _context.Brands.Add(person);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            //return View(person);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", brand.CountryId);
            ViewData["DealerId"] = new SelectList(_context.AuthorizedDealers, "Id", "DealerName", brand.DealerId);
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandViewModel brand)//[Bind("Id,BrandName,CountryId,DealerId,Description")] Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var person = await _context.Brands.Where(b => b.Id == brand.Id).FirstOrDefaultAsync();

                    person.Id = brand.Id;
                    person.BrandName = brand.BrandName;
                    person.BicycleModels = brand.BicycleModels;
                    person.Country = brand.Country;
                    person.CountryId = brand.CountryId;
                    person.Dealer = brand.Dealer;
                    person.DealerId = brand.DealerId;
                    person.Description = brand.Description;
                    
                    if (brand.Image != null)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(brand.Image.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)brand.Image.Length);
                        }
                        person.Image = imageData;
                    }
                    _context.Update(person);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "CountryName", brand.CountryId);
            ViewData["DealerId"] = new SelectList(_context.AuthorizedDealers, "Id", "DealerName", brand.DealerId);
            return View(brand);
        }

        // GET: Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .Include(b => b.Country)
                .Include(b => b.Dealer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _context.Brands.FindAsync(id);
            _context.Brands.Remove(brand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.Id == id);
        }

        public async Task<IActionResult> GoToModels(int? id)
        {
            RedirectToActionResult redirectToActionResult = RedirectToAction("Index", "BicycleModels", new { brand = id });
            return await Task.Run<IActionResult>(() =>
            {
                return redirectToActionResult;
            });
        }
    }
}
