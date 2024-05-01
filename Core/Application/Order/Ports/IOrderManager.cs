using Application.Order.Requests;
using Application.Order.Responses;

namespace Application.Order.Ports
{
    public interface IOrderManager
    {
        Task<OrderResponse> CreateOrder(CreateOrderRequest request);
        Task<OrderResponse> GetOrder(int id);
    }
}
