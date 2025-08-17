using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.Api.Models;

public class Sale
{
    public int SaleId { get; set; }

    public DateTime DateAndTimeOfSale { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal CalculatedTotal => LineItems.Sum(li => li.Quantity * li.UnitPrice);

    public List<LineItem> LineItems { get; } = [];
}
