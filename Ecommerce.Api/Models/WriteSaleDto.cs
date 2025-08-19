namespace Ecommerce.Api.Models;

public class WriteSaleDto
{
    public DateTime DateAndTimeOfSale { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal CalculatedTotal => LineItems.Sum(li => li.Quantity * li.UnitPrice);

    public List<LineItem> LineItems { get; } = [];
}
