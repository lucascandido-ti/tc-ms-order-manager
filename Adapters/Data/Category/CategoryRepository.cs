using Entities = Domain.Entities;

using Domain.Category.Ports;
using Domain.Entities;

namespace Data.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataDbContext _dbContext;

        public CategoryRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Entities.Category> CreateCategory(Entities.Category category)
        {
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public Task<Entities.Category> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Entities.Category>> List()
        {
            throw new NotImplementedException();
        }
    }
}
