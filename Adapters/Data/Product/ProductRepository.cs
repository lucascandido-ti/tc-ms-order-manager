using Entities = Domain.Entities;
using Domain.Product.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataDbContext _dbContext;

        public ProductRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Entities.Product> CreateProduct(Entities.Product product)
        {

            foreach (var category in product.Categories)
            {
                _dbContext.Entry(category).State = EntityState.Unchanged;
            }

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Entities.Product> Get(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            return product;
        }

        public async Task<List<Entities.Product>> List()
        {
            var products = await _dbContext.Products.ToListAsync();
            return products;
        }

        public async Task<Entities.Product> GetAggregate(int productId)
        {
            var products = await _dbContext.Products.Include(p => p.Categories).Where(p => p.Id == productId).FirstAsync();
            return products;
        }
    }
}
