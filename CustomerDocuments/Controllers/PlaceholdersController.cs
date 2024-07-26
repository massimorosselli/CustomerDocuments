using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CustomerDocuments.Models;

namespace CustomerDocuments.Controllers
{
    public class PlaceholdersController : Controller
    {
        private readonly CustomerContext _context;

        public PlaceholdersController(CustomerContext context)
        {
            _context = context;
        }

        // GET: Placeholders
        public async Task<IActionResult> Index()
        {
            var placeholders = await _context.Placeholders.ToListAsync();
            placeholders = placeholders.OrderBy(p => p.UserId).ThenBy(p => p.Name).ToList();
            return View(placeholders);
        }

        // GET: Placeholders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Placeholders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlaceholderId,UserId,Name,Value")] Placeholder placeholder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(placeholder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(placeholder);
        }

        // GET: Placeholders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placeholder = await _context.Placeholders.FindAsync(id);
            if (placeholder == null)
            {
                return NotFound();
            }
            return View(placeholder);
        }

        // POST: Placeholders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlaceholderId,UserId,Name,Value")] Placeholder placeholder)
        {
            if (id != placeholder.PlaceholderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(placeholder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlaceholderExists(placeholder.PlaceholderId))
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
            return View(placeholder);
        }

        // GET: Placeholders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var placeholder = await _context.Placeholders
                .FirstOrDefaultAsync(m => m.PlaceholderId == id);
            if (placeholder == null)
            {
                return NotFound();
            }

            return View(placeholder);
        }

        // POST: Placeholders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var placeholder = await _context.Placeholders.FindAsync(id);
            if (placeholder != null)
            {
                _context.Placeholders.Remove(placeholder);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlaceholderExists(int id)
        {
            return _context.Placeholders.Any(e => e.PlaceholderId == id);
        }
    }
}
