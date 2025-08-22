using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Repository;

public interface ILineItemRepository
{
    public Task<BaseResponse<List<LineItem>>> GetAllLineItems();

    public Task<BaseResponse<LineItem>> GetLineItemById(int id);

    public Task<BaseResponse<LineItem>> CreateLineItem(LineItem lineItem);

    public Task<BaseResponse<LineItem>> UpdateLineItem(LineItem updatedLineItem);

    public Task<BaseResponse<LineItem>> DeleteLineItem(int id);
}