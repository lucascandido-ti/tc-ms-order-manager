
using Application.Order.Responses;
using MediatR;

namespace Application.Order.Command
{
    public class SendOrderToProductionCommand: IRequest<OrderResponse>
    {
        public int orderId { get; set; }
    }
}
