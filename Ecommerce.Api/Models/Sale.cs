using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Api.Models;

public class Sale
{
    public int Id { get; set; }

    [Required]
    public DateTime TimeStamp { get; set; }

    [Required]
    public decimal Total { get; set; }

    [NotMapped]
    public decimal CalculatedTotal => LineItems.Sum(li => li.Quantity * li.UnitPrice);

    public List<LineItem> LineItems { get; } = [];
}
