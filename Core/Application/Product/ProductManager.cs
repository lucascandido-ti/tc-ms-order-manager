using Application.Product.Ports;
using Application.Product.Requests;
using Application.Product.Responses;

namespace Application.Product
{
    public class ProductManager : IProductManager
    {
        public Task<ProductResponse> CreateProduct(CreateProductRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ProductResponse> GetProduct(int id)
        {
            throw new NotImplementedException();
        }
    }
}
