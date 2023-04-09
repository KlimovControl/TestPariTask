using Microsoft.EntityFrameworkCore;
using Pari.Models;

namespace Pari.Data;

public class PariDbContext : DbContext
{
    public PariDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Bet> Bet { get; set; }
    public DbSet<PlayerStatus> PlayerStatus { get; set; }
}