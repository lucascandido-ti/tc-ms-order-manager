using Domain.Category.Exceptions;
using Domain.Category.Ports;

namespace Domain.Entities
{
    public class Category
    {
        public Category()
        {
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product>? Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        private void ValidateState()
        {
            if (this.Name == null)
            {
                throw new CategoryDescriptionRequiredException();
            }

            if (this.Description == null)
            {
                throw new CategoryNameRequiredException();
            }
        }

        public async Task Save(ICategoryRepository categoryRepository)
        {
            this.ValidateState();

            if (this.Id == 0)
            {
                var result = await categoryRepository.CreateCategory(this);
                this.Id = result.Id;
            }
        }
    }
}
