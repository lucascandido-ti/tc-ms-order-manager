﻿using Entities = Domain.Entities;
using Application.Order.Dto;
using Application.Order.Ports;
using Application.Order.Queries;
using Application.Order.Requests;
using Application.Order.Responses;
using Domain.Order.Exceptions;
using Domain.Order.Ports;
using Domain.Product.Ports;
using Domain.Utils;
using Application.Product.Dto;

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

    }
}
