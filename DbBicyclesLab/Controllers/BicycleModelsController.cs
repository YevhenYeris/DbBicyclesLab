using System;
using System.Reflection;
using System.Diagnostics;
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
using ClosedXML.Excel;
using Xceed.Words.NET;
using Xceed.Document.NET;
using Xceed.Words;
using Xceed.Document;

namespace DbBicyclesLab.Controllers
{
    public class BicycleModelsController : Controller
    {
        private readonly DBBicyclesContext _context;

        public BicycleModelsController(DBBicyclesContext context)
        {
            _context = context;
        }

        private IEnumerable<BicycleModel> ApplyFilters(int? brand, int? category, int? gender, int? minPrice, int? maxPrice, string year)
        {
            IEnumerable<BicycleModel> bicycleModels =  from m in _context.BicycleModels
                                                       .Include(x => x.SizeColorModels)
                                                       .Include($"{ nameof(DBBicyclesContext.SizeColorModels)}.{nameof(SizeColorModel.Size)}")
                                                       .Include($"{ nameof(DBBicyclesContext.SizeColorModels)}.{nameof(SizeColorModel.Color)}")
                                                       .Include($"{ nameof(DBBicyclesContext.SizeColorModels)}.{nameof(SizeColorModel.Bicycles)}")
                                                       .Include(x => x.Brand)
                                                       .Include(x => x.Category)
                                                       .Include(x => x.Gender)
                                                       select m;
            if (brand != null)
            {
                bicycleModels = bicycleModels.Where(b => b.BrandId == brand);
            }
            if (category != null)
            {
                bicycleModels = bicycleModels.Where(b => b.CategoryId == category);
            }
            if (gender != null)
            {
                bicycleModels = bicycleModels.Where(b => b.GenderId == gender);
            }
            if (year != null && year != "Усі")
            {
                bicycleModels = bicycleModels.Where(b => b.ModelYear == int.Parse(year));
            }
            if (maxPrice != null && minPrice != null)
                bicycleModels = bicycleModels.Where(b => b.Price >= (minPrice) && b.Price <= (maxPrice));
            return bicycleModels;
        }

        // GET: BicycleModels
        public async Task<IActionResult> Index(int? brand, int? category, int? gender, int? minPrice, int? maxPrice, string year)
        {
            BicycleModelsListModelView listModelView = new BicycleModelsListModelView(_context);
            listModelView.BicycleModels = ApplyFilters(brand, category, gender, minPrice, maxPrice, year);
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

        public ActionResult Export(int? brand, int? category, int? gender, int? minPrice, int? maxPrice, string year)
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var models = ApplyFilters(brand, category, gender, minPrice, maxPrice, year);

                foreach (var m in models)
                {
                    string name = m.ModelName.Length > 31 ? m.ModelName.Substring(0, 31) : m.ModelName;

                    var worksheet = workbook.Worksheets.Add(name);
                    worksheet.Cell("A1").Value = "Розмір";
                    worksheet.Cell("B1").Value = "Колір";
                    worksheet.Cell("C1").Value = "Опис";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var sizeColorModels = m.SizeColorModels;

                    for (int i = 0; i < sizeColorModels.Count; i++)
                    {
                        var scm = sizeColorModels.ElementAt(i);
                        string szName = scm.Size.SizeName;
                        string crName = scm.Color.ColorName;
                        for (int j = 0; j < sizeColorModels.ElementAt(i).Bicycles.Count; ++j)
                        {
                            worksheet.Cell(i + 2, 1).Value = szName;
                            worksheet.Cell(i + 2, 2).Value = crName;
                            worksheet.Cell(i + 2, 3).Value = scm.Bicycles.ElementAt(j).Description;
                        }
                        
                    }
                }
                return SaveDocXlsx(workbook);
            }
        }

        private ActionResult SaveDocXlsx(XLWorkbook workbook)
        {
            using (var stream = new MemoryStream())
            {
                if (!workbook.Worksheets.Any())
                    workbook.Worksheets.Add();

                workbook.SaveAs(stream);
                stream.Flush();

                return new FileContentResult(stream.ToArray(),
                   "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    FileDownloadName =
                   $"library_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                };
            }
        }

        public ActionResult ExportDocx(int? brand, int? category, int? gender, int? minPrice, int? maxPrice, string year)
        {
            //string fileName = @"bdb.docx";
            var doc = DocX.Create("");

            IEnumerable<Category> categories = category == null ? _context.Categories : new List<Category> { _context.Categories.Find(category) };

            foreach (var cat in categories)
            {
                List<BicycleModel> models = ApplyFilters(brand, cat.Id, gender, minPrice, maxPrice, year).ToList();

                if (models.Any())
                {
                    Paragraph paragraphTitle = doc.InsertParagraph(cat.CategoryName, false).FontSize(15D).Bold();
                    doc.InsertParagraph("\n");
                    paragraphTitle.Alignment = Alignment.center;

                    List<string> attributes = new List<string> { "Бренд", "Модель", "Рік", "Вартість, грн", "Для кого", "Країна" };
                    Table t = doc.AddTable(models.Count + 1, 6);
                    t.Alignment = Alignment.center;
                    t.Design = TableDesign.ColorfulList;

                    for (int i = 0; i < attributes.Count(); ++i)
                        t.Rows[0].Cells[i].Paragraphs.First().Append(attributes[i]);

                    int j = 1;
                    foreach (var m in models)
                    {
                        t.Rows[j].Cells[0].Paragraphs.First().Append(m.Brand.BrandName);
                        t.Rows[j].Cells[1].Paragraphs.First().Append(m.ModelName);
                        t.Rows[j].Cells[2].Paragraphs.First().Append(m.ModelYear.ToString());
                        t.Rows[j].Cells[3].Paragraphs.First().Append(m.Price.ToString());
                        t.Rows[j].Cells[4].Paragraphs.First().Append(m.Gender.GenderName);
                        t.Rows[j].Cells[5].Paragraphs.First().Append(_context.Countries.Find(m.Brand.CountryId).CountryName);

                        ++j;
                    }

                    doc.InsertTable(t);
                    doc.InsertParagraph("\n");
                }
            }
            return SaveDocDocx(doc);
        }

        private ActionResult SaveDocDocx(DocX doc)
        {
            using (var stream = new MemoryStream())
            {
                doc.SaveAs(stream);
                stream.Flush();
                return new FileContentResult(stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    FileDownloadName =
                $"library_{DateTime.UtcNow.ToShortDateString()}.docx"
                };
            }
        }
    }
}
