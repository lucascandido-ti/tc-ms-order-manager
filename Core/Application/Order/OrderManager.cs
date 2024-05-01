using Application.Order.Dto;
using Application.Order.Ports;
using Application.Order.Requests;
using Application.Order.Responses;
using Application.Product.Dto;
using Application.Product.Responses;
using Domain.Order.Exceptions;
using Domain.Order.Ports;
using Domain.Product.Ports;
using Domain.Utils;

namespace Application.Order
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderManager(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderResponse> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                var order = OrderDTO.MapToEntity(request.Data);

                await order.Save(_orderRepository);

                request.Data.Id = order.Id;

                return new OrderResponse
                {
                    Success = true,
                    Data = request.Data,
                };
            }
            catch (OrderProductsRequiredExceptions)
            {
                return new OrderResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ORDER_PRODUCTS_REQUIRED,
                    Message = "Products is required information"
                };
            }
        }

        public Task<OrderResponse> GetOrder(int id)
        {
            throw new NotImplementedException();
        }
    }
}
