namespace Ecommerce.Api.Models;

public class Sale
{
    public int Id { get; set; }

    public decimal Total { get; set; }

    public List<Product> Products { get; } = [];
}
