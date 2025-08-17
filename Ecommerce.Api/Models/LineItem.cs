using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Models;

public class LineItem
{
    public int LineItemId { get; set; }

    public int SaleId { get; set; }

    public Sale Sale { get; set; }

    public int ProductId { get; set; }

    public Product Product { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
