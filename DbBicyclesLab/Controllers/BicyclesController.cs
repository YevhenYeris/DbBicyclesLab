using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbBicyclesLab.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;

using Xceed.Words.NET;
using Xceed.Document.NET;
using Xceed.Words;
using Xceed.Document;

namespace DbBicyclesLab.Controllers
{
    public class BicyclesController : Controller
    {
        private readonly DBBicyclesContext _context;

        public BicyclesController(DBBicyclesContext context)
        {
            _context = context;
        }

        // GET: Bicycles
        public async Task<IActionResult> Index(int? size, int? color, int? model)
        {
            var dBBicyclesContext = _context.Bicycles.Include(b => b.SizeColorModel);
            //return View(await dBBicyclesContext.ToListAsync());

            BicycleListViewModel listViewModel = new BicycleListViewModel(_context);
            if (size != null)
            {
                /*FIX ASYNC*/
                listViewModel.Bicycles = listViewModel.Bicycles.Where(b => _context.SizeColorModels.Where(
                    s => s.Id == b.SizeColorModelId).Select(s => s.SizeId).FirstOrDefaultAsync().Result == size);
            }
            if (color != null)
            {
                listViewModel.Bicycles = listViewModel.Bicycles.Where(b => _context.SizeColorModels.Where(
                    s => s.Id == b.SizeColorModelId).Select(s => s.ColorId).FirstOrDefaultAsync().Result == color);
            }
            if (model != null)
            {
                listViewModel.Bicycles = listViewModel.Bicycles.Where(b => _context.SizeColorModels.Where(
                    s => s.Id == b.SizeColorModelId).Select(s => s.ModelId).FirstOrDefaultAsync().Result == model);
            }
            return View(listViewModel);
        }

        // GET: Bicycles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bicycle = await _context.Bicycles
                .Include(b => b.SizeColorModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bicycle == null)
            {
                return NotFound();
            }

            bicycle.SizeColorModel.Size = _context.Sizes.Find(bicycle.SizeColorModel.SizeId);
            bicycle.SizeColorModel.Color = _context.Colors.Find(bicycle.SizeColorModel.ColorId);
            bicycle.SizeColorModel.Model = _context.BicycleModels.Find(bicycle.SizeColorModel.ModelId);

            return View(bicycle);
        }

        // GET: Bicycles/Create
        public IActionResult Create(int? sizeColorModelId)
        {
            if (sizeColorModelId != null)
                ViewData["SizeColorModelId"] = new SelectList(_context.SizeColorModels, "Id", "Id", sizeColorModelId);
            else
                ViewData["SizeColorModelId"] = new SelectList(_context.SizeColorModels, "Id", "Id");
            return View();
        }

        // POST: Bicycles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,SizeColorModelId")] Bicycle bicycle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bicycle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SizeColorModelId"] = new SelectList(_context.SizeColorModels, "Id", "Id", bicycle.SizeColorModelId);
            return View(bicycle);
        }

        // GET: Bicycles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bicycle = await _context.Bicycles.FindAsync(id);
            if (bicycle == null)
            {
                return NotFound();
            }
            ViewData["SizeColorModelId"] = new SelectList(_context.SizeColorModels, "Id", "Id", bicycle.SizeColorModelId);
            return View(bicycle);
        }

        // POST: Bicycles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,SizeColorModelId")] Bicycle bicycle)
        {
            if (id != bicycle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bicycle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BicycleExists(bicycle.Id))
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
            ViewData["SizeColorModelId"] = new SelectList(_context.SizeColorModels, "Id", "Id", bicycle.SizeColorModelId);
            return View(bicycle);
        }

        // GET: Bicycles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bicycle = await _context.Bicycles
                .Include(b => b.SizeColorModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bicycle == null)
            {
                return NotFound();
            }

            return View(bicycle);
        }

        // POST: Bicycles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bicycle = await _context.Bicycles.FindAsync(id);
            _context.Bicycles.Remove(bicycle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BicycleExists(int id)
        {
            return _context.Bicycles.Any(e => e.Id == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            try
            {
                if (!ModelState.IsValid || fileExcel == null)
                    throw new Exception();

                using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                {
                    await fileExcel.CopyToAsync(stream);

                    using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                    {
                        foreach (IXLWorksheet worksheet in workBook.Worksheets)
                        {
                            ImportBicycles(worksheet);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                await Response.WriteAsync("<script>alert('" + e.Message + "');</script>");
            }
            return RedirectToAction(nameof(Index));
        }

        private void ImportBicycles(IXLWorksheet worksheet)
        {
            BicycleModel newmodel;
            var m = (from model in _context.BicycleModels
                     where model.ModelName.Contains(worksheet.Name)
                     select model).ToList();

            if (m.Any())
                newmodel = m[0];
            else
                throw new Exception("Model does not exist");

            foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
            {
                //Створимо модель
                SizeColorModel sizeColorModel = new SizeColorModel();
                sizeColorModel.Model = newmodel;

                //Шукаємо розмір в базі
                Size size = new Size();
                size.SizeName = row.Cell(1).Value.ToString();
                var s = (from sz in _context.Sizes
                         where sz.SizeName.Contains(size.SizeName)
                         select sz).ToList();

                if (!s.Any())
                    _context.Sizes.Add(size);
                else
                    size = s.First();
                sizeColorModel.Size = size;

                //Шукаємо колір в базі
                Color color = new Color();
                color.ColorName = row.Cell(2).Value.ToString();
                var c = (from cr in _context.Colors
                         where cr.ColorName.Equals(color.ColorName)
                         select cr).ToList();

                if (!c.Any())
                    _context.Colors.Add(color);
                else
                    color = c.First();
                sizeColorModel.Color = color;

                //Шукаємо колір-розмір-модель в базі
                var scm = (from o in _context.SizeColorModels
                           where o.Size.SizeName == size.SizeName
                           && o.Color.ColorName == color.ColorName
                           && o.Model.ModelName == newmodel.ModelName
                           select o).ToList();
                if (!scm.Any())
                    _context.SizeColorModels.Add(sizeColorModel);
                else
                    sizeColorModel = scm.First();

                Bicycle bicycle = new Bicycle { SizeColorModel = sizeColorModel, Description = row.Cell(3).Value.ToString() };

                //Шукаємо велосипед в базі
                var bcs = (from b in _context.Bicycles
                           where b.SizeColorModel == bicycle.SizeColorModel
                           && b.Description == bicycle.Description
                           select b).ToList();

                if (!bcs.Any())
                    _context.Bicycles.Add(bicycle);
                else
                {
                    var tbcc = bcs.First();
                    tbcc.Quantity = tbcc.Quantity == null ? 1 : tbcc.Quantity + 1;
                    _context.Bicycles.Update(tbcc);
                }
            }
        }

        /*public async Task<IActionResult> ImportDocX(IFormFile fileDocX)
        {
            if (!ModelState.IsValid || fileDocX == null)
                return RedirectToAction(nameof(Index));

            using (var stream = new FileStream(fileDocX.FileName, FileMode.Create))
            {
                await fileDocX.CopyToAsync(stream);

                DocX doc = DocX.Load(stream);

                foreach (var p in doc.Paragraphs)
                {
                    string catName = p.Text.Trim();

                    var cats = from c in _context.Categories where c.CategoryName == catName select c;
                    Category cat = new Category();

                    if (!cats.Any() && cats.Any())
                    {
                        cat.CategoryName = cats.First().CategoryName;
                        _context.Categories.Add(cat);
                    }

                    if (p.FollowingTables != null && p.FollowingTables.Any())
                    {
                        Table table = p.FollowingTables.First();

                        BicycleModel model = new BicycleModel();

                        foreach (var r in table.Rows.Skip(1))
                        {
                            string bname = r.Cells[0].ToString();
                            var brands = from b in _context.Brands where b.BrandName == bname select b;
                            Brand brand = new Brand();
                            if (brands.Any())
                            {
                                model.BrandId = brands.First().Id;
                            }

                            string mname = r.Cells[1].Paragraphs.First().Text;
                            model.ModelName = mname;

                            string yname = r.Cells[2].Paragraphs.First().Text;
                            model.ModelYear = int.Parse(yname);

                            string pname = r.Cells[3].Paragraphs.First().Text;
                            model.Price = int.Parse(pname);

                            string gname = r.Cells[4].Paragraphs.First().Text;
                            var genders = from g in _context.Genders where g.GenderName == gname select g;
                            Gender gender = new Gender();
                            if (genders.Any())
                            {
                                model.GenderId = genders.First().Id;
                            }

                            string cname = r.Cells[5].Paragraphs.First().Text;
                            var countries = from c in _context.Countries where c.CountryName == cname select c;
                            Country country = new Country();
                            if (countries.Any())
                            {
                                //model.Brand.CountryId = cous.First().Id;
                            }
                            _context.BicycleModels.Add(model);

                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }*/
    }
}
