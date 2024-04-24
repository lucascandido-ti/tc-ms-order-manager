using Domain.Product.Ports;

namespace Data.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataDbContext _dbContext;

        public ProductRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Entities.Product> CreateProduct(Domain.Entities.Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
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
