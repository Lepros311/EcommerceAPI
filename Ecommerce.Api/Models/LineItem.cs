using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Models;

public class LineItem
{
    public int LineItemId { get; set; }

    [Required]
    public int SaleId { get; set; }

    [Required]
    public Sale Sale { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    public Product Product { get; set; }

    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal UnitPrice { get; set; }
}
