using Application.Order.Queries;
using Application.Order.Requests;
using Application.Order.Responses;

namespace Application.Order.Ports
{
    public interface IOrderManager
    {
        Task<OrderResponse> CreateOrder(CreateOrderRequest request);
        Task<OrderResponse> SendOrderToProduction(SendOrderToProductionRequest request);
        Task<OrderResponse> StartProduction(StartProductionRequest request);
        Task<OrderResponse> ConcludedProduction(ConcludedProductionRequest request);
        Task<OrderResponse> GetOrder(GetOrderQuery query);
        Task<ListOrderResponse> GetOrders();
    }
}
