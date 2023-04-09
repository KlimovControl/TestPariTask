using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace Pari.Models;

public class Player
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [DisplayName("Имя")]
    public string FirstName { get; set; }
    
    [Required]
    [DisplayName("Фамилия")]
    public string SecondName { get; set; }
    
    [DisplayName("Отчество")]
    public string MiddleName { get; set; }

    [DisplayName("ФИО")]
    [NotMapped]
    public string AbbriviatedFullName
    {
        get { return $"{SecondName} {FirstName[0]}. {MiddleName[0]}."; }
    }

    [DisplayName("Баланс")]
    public decimal Balance { get; set; }
    
    [DisplayName("Дата регистрации")]
    public DateTime RegistrationDate  { get; set; } = DateTime.Now;
    
    [DisplayName("Статус")]
    public PlayerStatus? PlayerStatus { get; set;}
    
    [DisplayName("Статус")]
    public byte? PlayerStatusId { get; set; }

    public IEnumerable<Bet>? Bets { get; set; }
    public IEnumerable<Transaction>? Transactions { get; set; }

}
