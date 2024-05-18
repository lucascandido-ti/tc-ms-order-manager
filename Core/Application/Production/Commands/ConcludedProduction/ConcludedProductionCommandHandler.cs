using Application.Order.Ports;
using Application.Order.Requests;
using Application.Order.Responses;
using MediatR;

namespace Application.Production.Commands { 
    public class ConcludedProductionOrderCommandHandler : IRequestHandler<ConcludedProductionCommand, OrderResponse>
    {
        private readonly IOrderManager _orderManager;
        public ConcludedProductionOrderCommandHandler(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public async Task<OrderResponse> Handle(ConcludedProductionCommand request, CancellationToken cancellationToken)
        {
            var command = new ConcludedProductionRequest()
            {
                orderId = request.productionDTO.order.orderId
            };

            return await _orderManager.ConcludedProduction(command);
        }
    }
}
