
using Application.Order.Ports;
using Application.Order.Requests;
using Application.Order.Responses;
using MediatR;

namespace Application.Order.Command
{
    public class SendOrderToProductionCommandHandler : IRequestHandler<SendOrderToProductionCommand, OrderResponse>
    {


        private readonly IOrderManager _orderManager;

        public SendOrderToProductionCommandHandler(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public Task<OrderResponse> Handle(SendOrderToProductionCommand request, CancellationToken cancellationToken)
        {
            var payload = new SendOrderToProductionRequest
            {
                orderId = request.orderId
            };

            return _orderManager.SendOrderToProduction(payload);
        }
    }
}
