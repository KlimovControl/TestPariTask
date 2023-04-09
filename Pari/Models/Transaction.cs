using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Pari.Data;

namespace Pari.Models;

public class Transaction
{
    [Key]
    [DisplayName("ID транзакции")]
    public int Id { get; set; }
        
    [DisplayName("Игрок")]
    public Player? Player { get; set; }

    [Required]
    [DisplayName("Игрок")]
    public int PlayerId { get; set; }
    
    [Required]
    [DisplayName("Сумма")]
    public decimal TransactionAmount { get; set; }
    
    [DisplayName("Дата проведения")]
    public DateTime RegistrationDate  { get; set; } = DateTime.Now;
    
    [DisplayName("Тип транзакции")]
    public TransactionType TransactionType { get; set; }
}