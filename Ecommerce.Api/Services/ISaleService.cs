using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public interface ISaleService
{
    Task<BaseResponse<List<SaleDto>>> GetAllSales();

    Task<BaseResponse<Sale>> GetSaleById(int id);

    Task<BaseResponse<SaleDto>> CreateSale(WriteSaleDto sale);

    Task<BaseResponse<Sale>> UpdateSale(int id, WriteSaleDto sale);

    Task<BaseResponse<Sale>> DeleteSale(int id);
}


