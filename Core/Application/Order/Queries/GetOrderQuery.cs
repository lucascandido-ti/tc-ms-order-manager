using Application.Order.Responses;
using MediatR;

namespace Application.Order.Queries
{
    public class GetOrderQuery: IRequest<OrderResponse>
    {
        public int Id { get; set; }
    }
}
