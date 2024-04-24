using Application.Product.Dto;
using Application.Product.Responses;
using MediatR;

namespace Application.Product.Commands
{
    public class CreateProductCommand: IRequest<ProductResponse>
    {
        public ProductDTO productDTO { get; set; }
    }
}
