using System.ComponentModel.DataAnnotations;

namespace Pari.Models;

public class PlayerStatus
{
    [Key]
    public byte Id { get; set; }
    [Required]
    public string StatusName { get; set; }
    public byte? DiscountRate { get; set; }
    public string? Description { get; set; }
}