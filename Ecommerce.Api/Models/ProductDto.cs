namespace Ecommerce.Api.Models;

public class ProductDto
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public decimal Price { get; set; }

    public string? Category { get; set; }
}
