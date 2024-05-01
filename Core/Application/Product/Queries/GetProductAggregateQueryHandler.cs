using Application.Product.Ports;
using Application.Product.Responses;
using MediatR;

namespace Application.Product.Queries
{
    public class GetProductAggregateQueryHandler : IRequestHandler<GetProductAggregateQuery, ProductResponse>
    {

        private readonly IProductManager _productManager;

        public GetProductAggregateQueryHandler(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<ProductResponse> Handle(GetProductAggregateQuery request, CancellationToken cancellationToken)
        {
            return await _productManager.GetProductAggregate(request);
        }
    }
}
