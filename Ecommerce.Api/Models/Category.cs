using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Models;

public class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public List<Product> Products { get; } = [];

    public override string ToString() => $"{CategoryName}";
}
