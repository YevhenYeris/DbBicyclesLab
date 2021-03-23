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
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);

                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            try
                            {
                                foreach (IXLWorksheet worksheet in workBook.Worksheets)
                                {
                                    BicycleModel newmodel;
                                    var m = (from model in _context.BicycleModels
                                             where model.ModelName.Contains(worksheet.Name)
                                             select model).ToList();
                                    if (m.Any())
                                    {
                                        newmodel = m[0];
                                    }
                                    else
                                    {
                                        throw new Exception("Model does not exist");
                                        /*newmodel = new BicycleModel();
                                        newmodel.ModelName = worksheet.Name;
                                        newmodel.Description = "from EXCEL";
                                        //додати в контекст
                                        _context.BicycleModels.Add(newmodel);*/
                                    }
                                    foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                    {

                                        Size size = new Size();
                                        size.SizeName = row.Cell(1).Value.ToString();
                                        var s = (from sz in _context.Sizes
                                                 where sz.SizeName.Contains(size.SizeName)
                                                 select sz).ToList();

                                        if (!s.Any())
                                            _context.Sizes.Add(size);
                                        else
                                            size = s[0];

                                        Color color = new Color();
                                        color.ColorName = row.Cell(2).Value.ToString();
                                        var c = (from cr in _context.Colors
                                                 where cr.ColorName.Equals(color.ColorName)
                                                 select cr).ToList();

                                        if (!c.Any())
                                            _context.Colors.Add(color);
                                        else
                                            color = c[0];

                                        SizeColorModel sizeColorModel = new SizeColorModel() { Size = size, Color = color, Model = newmodel };
                                        _context.SizeColorModels.Add(sizeColorModel);

                                        Bicycle bicycle = new Bicycle { SizeColorModel = sizeColorModel, Description = row.Cell(3).Value.ToString() };
                                        _context.Bicycles.Add(bicycle);

                                        await _context.SaveChangesAsync();
                                    }

                                }
                            }
                            catch (Exception e)
                            {
                                await Response.WriteAsync("<script>alert('"+ e.Message + "');</script>");
                            }
                        }
                        
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
