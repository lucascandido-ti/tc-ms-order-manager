using Application.Order.Ports;
using Application.Order.Requests;
using Application.Order.Responses;
using MediatR;

namespace Application.Order.Command.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderResponse>
    {
        private readonly IOrderManager _orderManager;

        public CreateOrderCommandHandler(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public async Task<OrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var command = new CreateOrderRequest()
            {
                Data = request.orderDTO
            };

            return await _orderManager.CreateOrder(command);
        }
    }
}
