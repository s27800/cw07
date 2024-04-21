using System.ComponentModel.DataAnnotations;

namespace cwiczenia07.Models.DTOs;

public class AddProduct
{
    [Required]
    public int IdProduct { get; set; }
    [Required]
    public int IdWarehouse { get; set; }
    [Required]
    public int Amount { get; set; }
    [Required]
    public string CreatedAt { get; set; }
}