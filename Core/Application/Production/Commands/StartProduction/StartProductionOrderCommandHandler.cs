using Application.Order.Ports;
using Application.Order.Requests;
using Application.Order.Responses;
using MediatR;

namespace Application.Production.Commands.StartProduction
{
    public class StartProductionOrderCommandHandler : IRequestHandler<StartProductionOrderCommand, OrderResponse>
    {
        private readonly IOrderManager _orderManager;
        public StartProductionOrderCommandHandler(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public async Task<OrderResponse> Handle(StartProductionOrderCommand request, CancellationToken cancellationToken)
        {
            var command = new StartProductionRequest()
            {
                orderId = request.productionDTO.order.orderId
            };

            return await _orderManager.StartProduction(command);
        }
    }
}
