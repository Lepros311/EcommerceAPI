using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Api.Models;

public class Sale
{
    public int SaleId { get; set; }

    [Required]
    public DateTime DateAndTimeOfSale { get; set; }

    [Required]
    public decimal TotalPrice { get; set; }

    [NotMapped]
    public decimal CalculatedTotal => LineItems.Sum(li => li.Quantity * li.UnitPrice);

    public List<LineItem> LineItems { get; } = [];
}
