using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Models;

public class Sale
{
    public int Id { get; set; }

    [Required]
    public DateTime TimeStamp { get; set; }

    [Required]
    public decimal Total { get; set; }

    [Required]
    public List<Product> Products { get; } = [];
}
