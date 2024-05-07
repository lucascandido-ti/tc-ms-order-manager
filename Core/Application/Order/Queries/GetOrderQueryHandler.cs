using Application.Order.Ports;
using Application.Order.Responses;
using MediatR;

namespace Application.Order.Queries
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, OrderResponse>
    {
        private readonly IOrderManager _orderManager;

        public GetOrderQueryHandler(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public async Task<OrderResponse> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            return await _orderManager.GetOrder(request);
        }
    }
}
