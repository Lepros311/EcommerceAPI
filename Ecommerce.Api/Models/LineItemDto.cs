namespace Ecommerce.Api.Models;

public class LineItemDto
{
    public int LineItemId { get; set; }

    public int SaleId { get; set; }

    public Sale Sale { get; set; }

    public int ProductId { get; set; }

    public ProductDto ProductDto { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
