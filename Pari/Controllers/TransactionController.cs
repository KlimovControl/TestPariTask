using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pari.Data;
using Pari.Models;

namespace Pari.Controllers
{
    public class TransactionController : Controller
    {
        private readonly PariDbContext _context;

        public TransactionController(PariDbContext context)
        {
            _context = context;
        }

        // GET
        public async Task<IActionResult> Index()
        {
            var pariDbContext = _context.Transactions.Include(t => t.Player);
            return View(await pariDbContext.ToListAsync());
        }

        // GET
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Player)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET
        public IActionResult Create()
        {
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "AbbriviatedFullName");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlayerId,TransactionAmount,RegistrationDate,TransactionType")] Transaction transaction)
        {
            var player = await _context.Players.FirstOrDefaultAsync(p => p.Id == transaction.PlayerId);
            if (player == null)
            {
                return NotFound();
            }
            if (!ChangeBalance(player, transaction.TransactionAmount, (byte)transaction.TransactionType))
            {
                ModelState.AddModelError(nameof(transaction.TransactionAmount),ErrorMessage.WithdrawalError);
            }

            if (ModelState.IsValid)
            {
                _context.Update(player);
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "AbbriviatedFullName", transaction.PlayerId);
            return View(transaction);
        }

        // GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "AbbriviatedFullName", transaction.PlayerId);
            return View(transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlayerId,TransactionAmount,RegistrationDate,TransactionType")] Transaction transaction)
        {
            if (id != transaction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.Id))
                    {
                        return NotFound();
                    }
                    
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "FirstName", transaction.PlayerId);
            return View(transaction);
        }

        // GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Player)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
          return _context.Transactions.Any(e => e.Id == id);
        }
        
        private bool ChangeBalance (Player player, decimal transactionAmount, byte transactionType)
        {
            switch (transactionType)
            {
                case (byte)TransactionType.Deposit:
                    player.Balance = Decimal.Add(player.Balance, transactionAmount);
                    break;
                case (byte)TransactionType.Withdrawal when player.Balance >= transactionAmount:
                    player.Balance = Decimal.Subtract(player.Balance, transactionAmount);
                    break;
                default:
                    return false;
            }
            return true;
        } 
    }
}
