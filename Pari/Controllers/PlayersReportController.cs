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
                .AsNoTracking()
                .Include(p => p.PlayerStatus)
                .Include(p=> p.Bets)
                .Include(p=> p.Transactions!
                    .Where(tt => tt.TransactionType==TransactionType.Deposit))
                .AsQueryable();
            
            List<PlayerStatus> statuses = await _context.PlayerStatus.AsNoTracking().ToListAsync();
            statuses.Insert(0, new PlayerStatus {StatusName = "Все", Id = 0});
            
            
            if (status != null && status!=0)
            {
                players = players.Where(p => p.PlayerStatus!.Id == status);
            }
            var resultPlayersList = new List<PlayersReportModel>();
            
            foreach (var player in players)
            {
                var playerTransactionsSum = player.Transactions!.Sum(ta => ta.TransactionAmount);
                var playerBetsSum = player.Bets!.Sum(ba => ba.BetAmount);
                var isBetBiggerThanTransaction = playerBetsSum > playerTransactionsSum;
                
                switch (isBetHigherChecked)
                {
                    case false:
                    case true when isBetBiggerThanTransaction:
                    {
                        var playerToList = new PlayersReportModel
                        {
                            Id = player.Id,
                            AbbreviatedFullName = player.AbbriviatedFullName,
                            Balance = player.Balance,
                            RegistrationDate = player.RegistrationDate,
                            Status = player.PlayerStatus,
                            DepositAmount = playerTransactionsSum,
                            BetAmount = playerBetsSum
                        };
                        resultPlayersList.Add(playerToList);
                        break;
                    }
                }
            }
            
            var resultModel = new PlayersReportViewModel
            {
                PlayersReportModels = resultPlayersList,
                isBetHigher = isBetHigherChecked,
                PlayerStatuses = new SelectList(statuses, "Id", "StatusName")
            };
            return View(resultModel);
        }
    }
