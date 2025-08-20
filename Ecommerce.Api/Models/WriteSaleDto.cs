namespace Ecommerce.Api.Models;

public class WriteSaleDto
{
    public DateTime DateAndTimeOfSale { get; set; }

    public List<WriteLineItemDto> LineItems { get; set; } = new List<WriteLineItemDto>();
}
