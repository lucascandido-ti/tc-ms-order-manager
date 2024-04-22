using Application.Product.Requests;
using Application.Product.Responses;

namespace Application.Product.Ports
{
    public interface IProductManager
    {
        Task<ProductResponse> CreateProduct(CreateProductRequest request);
        Task<ProductResponse> GetProduct(int id);
    }
}
