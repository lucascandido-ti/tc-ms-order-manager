using Application.Product.Responses;
using MediatR;

namespace Application.Product.Queries
{
    public class GetProductQuery: IRequest<ProductResponse>
    {
        public int Id { get; set; }
    }
}
