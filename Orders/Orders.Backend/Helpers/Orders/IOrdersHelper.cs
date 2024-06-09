using Orders.Shared.Responses;

namespace Orders.Backend.Helpers.Orders
{
    public interface IOrdersHelper
    {
        Task<ActionResponse<bool>> ProcessOrderAsync(string email, string remarks);
    }
}
