using Application.Product.Ports;
using Application.Product.Requests;
using Application.Product.Responses;
using MediatR;

namespace Application.Product.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductResponse>
    {

        private readonly IProductManager _productManager;

        public CreateProductCommandHandler(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var req = new CreateProductRequest
            {
                Data = request.productDTO
            };

            return _productManager.CreateProduct(req);
        }
    }
}
