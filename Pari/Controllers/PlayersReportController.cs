using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pari.Data;
using Pari.Models;

namespace Pari.Controllers;

    public class PlayersReportController : Controller
    {
        private readonly PariDbContext _context;

        public PlayersReportController(PariDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(byte? status, bool isBetHigherChecked)
        {
            var players = _context.Players
                .Include(p => p.Bets)
                .Include(p=>p.Transactions)
                .Select(p=>
            new PlayersReportModel
                {Id = p.Id, 
                    AbbreviatedFullName = p.AbbriviatedFullName, 
                    Balance = p.Balance, 
                    Status = p.PlayerStatus, 
                    RegistrationDate = p.RegistrationDate, 
                    BetAmount = (decimal)p.Bets!.Sum(ba => (float)ba.BetAmount),
                    DepositAmount = (decimal)p.Transactions!
                        .Where(ta=>ta.TransactionType == TransactionType.Deposit)
                        .Sum(ta => (float)ta.TransactionAmount)
                })
                .AsQueryable();
            
            List<PlayerStatus> statuses = await _context.PlayerStatus.AsNoTracking().ToListAsync();
            statuses.Insert(0, new PlayerStatus {StatusName = "Все", Id = 0});
            
            if (status != null && status!=0)
            {
                players = players.Where(p => p.Status!.Id == status);
            }

            if (isBetHigherChecked)
            {
                players = players.Where(p => p.BetAmount > p.DepositAmount);
            }

            var resultModel = new PlayersReportViewModel
            {
                PlayersReportModels = players,
                isBetHigher = isBetHigherChecked,
                PlayerStatuses = new SelectList(statuses, "Id", "StatusName")
            };
            return View(resultModel);
        }
    }
