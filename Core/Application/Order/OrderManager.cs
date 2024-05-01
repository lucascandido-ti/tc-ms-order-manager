using Application.Order.Ports;
using Application.Order.Requests;
using Application.Order.Responses;

namespace Application.Order
{
    public class OrderManager : IOrderManager
    {
        public Task<OrderResponse> CreateOrder(CreateOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<OrderResponse> GetOrder(int id)
        {
            throw new NotImplementedException();
        }
    }
}
