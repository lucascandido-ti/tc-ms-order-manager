using Application.Order.Responses;
using MediatR;

namespace Application.Order.Queries
{
    public class ListOrdersQuery: IRequest<ListOrderResponse>
    {
    }
}
