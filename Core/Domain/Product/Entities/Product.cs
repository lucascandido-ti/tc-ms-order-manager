using Domain.Product.Exceptions;
using Domain.Product.Ports;
using Domain.Utils.ValueObjects;

namespace Domain.Entities
{
    public class Product
    {
        public Product()
        {
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Price Price { get; set; }
        public ICollection<Category> Categories { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        private void ValidateState()
        {
            if (this.Name == null)
            {
                throw new ProductNameRequiredException();
            }

            if (this.Price == null || this.Price.Value < 1)
            {
                throw new ProductPriceRequiredException();
            }

        }

        public async Task Save(IProductRepository productRepository)
        {
            this.ValidateState();

            if (this.Id == 0)
            {
                var result = await productRepository.CreateProduct(this);
                this.Id = result.Id;
            }
        }
    }
}
