using Application.Order.Dto;
using Application.Order.Responses;
using MediatR;

namespace Application.Order.Command
{
    public class CreateOrderCommand: IRequest<OrderResponse>
    {
        public OrderDTO orderDTO { get; set; }
    }
}
