using Application.Product.Ports;
using Application.Product.Responses;
using MediatR;

namespace Application.Product.Queries
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductResponse>
    {
        private readonly IProductManager _productManager;

        public GetProductQueryHandler(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<ProductResponse> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            return await _productManager.GetProduct(request);
        }
    }
}
