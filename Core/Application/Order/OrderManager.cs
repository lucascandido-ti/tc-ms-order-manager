using Application.Order.Dto;
using Application.Order.Events;
using Application.Order.Ports;
using Application.Order.Queries;
using Application.Order.Requests;
using Application.Order.Responses;
using Domain.Order.Enums;
using Domain.Order.Exceptions;
using Domain.Order.Ports;
using Domain.Queue.Ports;
using Domain.Utils;

namespace Application.Order
{
    public class OrderManager : IOrderManager
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IQueueRepository _rabbitMQRepository;

        public OrderManager(
            IOrderRepository orderRepository,
            IQueueRepository rabbitMQRepository
        )
        {
            _orderRepository = orderRepository;
            _rabbitMQRepository = rabbitMQRepository;
        }

        public async Task<OrderResponse> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                var order = OrderDTO.MapToEntity(request.Data);

                await order.Save(_orderRepository);

                var orderDto = OrderDTO.MapToDTO(order);

                var createOrderEvent = new CreateOrderEvent()
                {
                    Order = orderDto
                };

                _rabbitMQRepository.Publish(createOrderEvent.Order, "created-new-order", "order-service-queue");

                return new OrderResponse
                {
                    Success = true,
                    Data = orderDto,
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

        public async Task<OrderResponse> GetOrder(GetOrderQuery query)
        {
            var order = await _orderRepository.Get(query.Id);

            if (order == null)
            {
                return new OrderResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.ORDER_NOT_FOUND,
                    Message = "No order record was found with the given Id"
                };
            }

            var orderDto = OrderDTO.MapToDTO(order);

            return new OrderResponse
            {
                Success = true,
                Data = orderDto
            };
        }

        public async Task<ListOrderResponse> GetOrders()
        {
            var orders = await _orderRepository.List();

            if (orders == null || orders.Count == 0)
            {
                return new ListOrderResponse { };
            }

            var listOrders = new List<OrderDTO>();

            foreach (var order in orders)
            {
                listOrders.Add(OrderDTO.MapToDTO(order));
            }

            return new ListOrderResponse
            {
                Success = true,
                Data = listOrders
            };
        }

        public async Task<OrderResponse> SendOrderToProduction(SendOrderToProductionRequest request)
        {
            var query = await GetOrder(new GetOrderQuery { Id = request.orderId });

            var orderEntity = OrderDTO.MapToEntity(query.Data);

            var orderUpdated = await _orderRepository.UpdateStatus(orderEntity, OrderStatus.RECEIVED);

            var orderDto = OrderDTO.MapToDTO(orderUpdated);

            _rabbitMQRepository.Publish(orderDto, "send-order-to-production", "order-service-queue");

            return new OrderResponse
            {
                Success = true,
                Data = orderDto
            };
            
        }

        public async Task<OrderResponse> StartProduction(StartProductionRequest request)
        {
            var query = await GetOrder(new GetOrderQuery { Id = request.orderId });

            var orderEntity = OrderDTO.MapToEntity(query.Data);

            var orderUpdated = await _orderRepository.UpdateStatus(orderEntity, OrderStatus.IN_PREPARATION);

            var orderDto = OrderDTO.MapToDTO(orderUpdated);

            return new OrderResponse
            {
                Success = true,
                Data = orderDto
            };
        }

        public async Task<OrderResponse> ConcludedProduction(ConcludedProductionRequest request)
        {
            var query = await GetOrder(new GetOrderQuery { Id = request.orderId });

            var orderEntity = OrderDTO.MapToEntity(query.Data);

            var orderUpdated = await _orderRepository.UpdateStatus(orderEntity, OrderStatus.CONCLUDED);

            var orderDto = OrderDTO.MapToDTO(orderUpdated);

            return new OrderResponse
            {
                Success = true,
                Data = orderDto
            };
        }

    }
}
