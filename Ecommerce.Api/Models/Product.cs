using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Models;

public class Product
{
    public int ProductId { get; set; }

    [Required]
    public string ProductName { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [Required]
    public Category Category { get; set; }

    public List<LineItem> LineItems { get; } = [];

    [Required]
    public bool IsDeleted { get; set; } = false;
}
