namespace Ecommerce.Api.Models;

public class WriteLineItemDto
{
    public int? LineItemId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }
}
