using System.ComponentModel.DataAnnotations;

namespace Pari.Data;

public enum TransactionType : byte
{
    [Display(Name = "Снятие")]
    Withdrawal = 0,
    [Display(Name = "Внесение")]
    Deposit = 1
}