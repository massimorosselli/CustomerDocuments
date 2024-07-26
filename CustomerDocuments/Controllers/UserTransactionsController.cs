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
    public class UserTransactionsController : Controller
    {
        private readonly CustomerContext _context;

        public UserTransactionsController(CustomerContext context)
        {
            _context = context;
        }

        // GET: UserTransactions
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserTransactions.ToListAsync());
        }
        
        // GET: UserTransactions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserTransactionId,UserId,DocumentId,TransactionDate")] UserTransaction userTransaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userTransaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userTransaction);
        }

        // GET: UserTransactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTransaction = await _context.UserTransactions.FindAsync(id);
            if (userTransaction == null)
            {
                return NotFound();
            }
            return View(userTransaction);
        }

        // POST: UserTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserTransactionId,UserId,DocumentId,TransactionDate")] UserTransaction userTransaction)
        {
            if (id != userTransaction.UserTransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userTransaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserTransactionExists(userTransaction.UserTransactionId))
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
            return View(userTransaction);
        }

        // GET: UserTransactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userTransaction = await _context.UserTransactions
                .FirstOrDefaultAsync(m => m.UserTransactionId == id);
            if (userTransaction == null)
            {
                return NotFound();
            }

            return View(userTransaction);
        }

        // POST: UserTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userTransaction = await _context.UserTransactions.FindAsync(id);
            if (userTransaction != null)
            {
                _context.UserTransactions.Remove(userTransaction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserTransactionExists(int id)
        {
            return _context.UserTransactions.Any(e => e.UserTransactionId == id);
        }
    }
}
