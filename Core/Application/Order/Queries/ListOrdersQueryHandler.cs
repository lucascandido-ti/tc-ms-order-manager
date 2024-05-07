using Application.Order.Ports;
using Application.Order.Responses;
using MediatR;

namespace Application.Order.Queries
{
    public class ListOrdersQueryHandler : IRequestHandler<ListOrdersQuery, ListOrderResponse>
    {
        private readonly IOrderManager _orderManager;

        public ListOrdersQueryHandler(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public async Task<ListOrderResponse> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
        {
            return await _orderManager.GetOrders();
        }
    }
}
