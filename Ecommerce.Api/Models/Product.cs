using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public string Name { get; set; }

    public Category? Category { get; set; }

    public List<Sale> Sales { get; } = [];
}
