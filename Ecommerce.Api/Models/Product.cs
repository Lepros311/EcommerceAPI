namespace Ecommerce.Api.Models;

public class Product
{
    public int Id { get; set; }

    public decimal Price { get; set; }

    public string Name { get; set; }

    public Category Category { get; set; }

    public List<Sale> Sales { get; } = [];
}
