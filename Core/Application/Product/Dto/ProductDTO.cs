using Domain.Utils.Enums;
using Entities = Domain.Entities;
using ValueObjects = Domain.Product.ValueObjects;

namespace Application.Product.Dto
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public AcceptedCurrencies Currency { get; set; }

        public static Entities.Product MapToEntity(ProductDTO dto)
        {
            return new Entities.Product
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = new ValueObjects.Price { Currency = dto.Currency, Value = dto.Price },
            };
        }

        public static ProductDTO MapToDTO(Entities.Product product)
        {
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.Value
            };
        }
    }
}
