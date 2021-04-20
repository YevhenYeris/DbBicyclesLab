using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbBicyclesLab.Models;
using Microsoft.AspNetCore.Authorization;

namespace DbBicyclesLab.Controllers
{
    [Authorize(Roles = "admin")]
    public class AuthorizedDealersController : Controller
    {
        private readonly DBBicyclesContext _context;

        public AuthorizedDealersController(DBBicyclesContext context)
        {
            _context = context;
        }

        // GET: AuthorizedDealers
        public async Task<IActionResult> Index()
        {
            return View(await _context.AuthorizedDealers.ToListAsync());
        }

        // GET: AuthorizedDealers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorizedDealer = await _context.AuthorizedDealers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorizedDealer == null)
            {
                return NotFound();
            }

            return View(authorizedDealer);
        }

        // GET: AuthorizedDealers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AuthorizedDealers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DealerName,WebsiteAddress,Description")] AuthorizedDealer authorizedDealer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(authorizedDealer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(authorizedDealer);
        }

        // GET: AuthorizedDealers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorizedDealer = await _context.AuthorizedDealers.FindAsync(id);
            if (authorizedDealer == null)
            {
                return NotFound();
            }
            return View(authorizedDealer);
        }

        // POST: AuthorizedDealers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DealerName,WebsiteAddress,Description")] AuthorizedDealer authorizedDealer)
        {
            if (id != authorizedDealer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authorizedDealer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorizedDealerExists(authorizedDealer.Id))
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
            return View(authorizedDealer);
        }

        // GET: AuthorizedDealers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorizedDealer = await _context.AuthorizedDealers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authorizedDealer == null)
            {
                return NotFound();
            }

            return View(authorizedDealer);
        }

        // POST: AuthorizedDealers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var authorizedDealer = await _context.AuthorizedDealers.FindAsync(id);
            _context.AuthorizedDealers.Remove(authorizedDealer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorizedDealerExists(int id)
        {
            return _context.AuthorizedDealers.Any(e => e.Id == id);
        }
    }
}
