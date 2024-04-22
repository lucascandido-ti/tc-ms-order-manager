using Domain.Product.Ports;

namespace Data.Product
{
    public class ProductRepository : IProductRepository
    {
        public Task<Domain.Entities.Product> CreateProduct(Domain.Entities.Product product)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Entities.Product> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Domain.Entities.Product>> List()
        {
            throw new NotImplementedException();
        }
    }
}
