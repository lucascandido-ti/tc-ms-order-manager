using Application.Product.Dto;
using Application.Product.Queries;
using Application.Product.Requests;
using Application.Product.Responses;

namespace Application.Product.Ports
{
    public interface IProductManager
    {
        Task<ProductResponse> CreateProduct(CreateProductRequest request);
        Task<ProductResponse> GetProduct(GetProductQuery get);
        Task<List<ProductDTO>> GetProducts();
        Task<ProductResponse> GetProductAggregate(GetProductQuery get);
    }
}
