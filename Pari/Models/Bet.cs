using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pari.Models;

public class Bet
{
    [Key]
    public int Id { get; set; }
    
    [DisplayName("Игрок")]
    public Player? Player { get; set; }
    
    [Required]
    [DisplayName("Игрок")]
    public int PlayerId { get; set; }
    
    [Required]
    [DisplayName("Сумма")]
    public decimal BetAmount { get; set; }
    
    [DisplayName("Дата ставки")]
    public DateTime BetDate  { get; set; } = DateTime.Now;
    
    [DisplayName("Выйгрыш")]
    public decimal WinningsAmount { get; set; }
    
    [DisplayName("Дата расчета")]
    public DateTime SettlementDate  { get; set; }
}