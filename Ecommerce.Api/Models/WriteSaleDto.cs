namespace Ecommerce.Api.Models;

public class WriteSaleDto
{
    public DateTime DateAndTimeOfSale { get; set; }

    public List<WriteLineItemOnSaleDto> LineItems { get; set; } = new List<WriteLineItemOnSaleDto>();
}
