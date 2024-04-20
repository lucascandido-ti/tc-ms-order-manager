using Entities = Domain.Entities;

using Domain.Category.Ports;

namespace Data.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        public Task<Entities.Category> CreateCategory(Entities.Category category)
        {
            throw new NotImplementedException();
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
