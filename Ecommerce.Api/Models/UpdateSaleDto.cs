namespace Ecommerce.Api.Models;

public class UpdateSaleDto
{
    public DateTime DateAndTimeOfSale { get; set; }

    public List<UpdateLineItemOnSaleDto> LineItems { get; set; } = new List<UpdateLineItemOnSaleDto>();
}
