using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Pari.Models;

public class PlayersReportModel
{
    [Key]
    public int Id { get; set; }
    [Required]
    
    [DisplayName("ФИО")]
    public string AbbreviatedFullName { get; set; }
    
    [DisplayName("Баланс")]
    public decimal Balance { get; set; }
    
    [DisplayName("Дата регистрации")]
    public DateTime RegistrationDate  { get; set; }
    
    [DisplayName("Статус")]
    public PlayerStatus?  Status { get; set;}
    
    [Required]
    [DisplayName("Сумма внесений ")]
    public decimal DepositAmount { get; set; }
    
    [Required]
    [DisplayName("Сумма ставок")]
    public decimal BetAmount { get; set; }
}