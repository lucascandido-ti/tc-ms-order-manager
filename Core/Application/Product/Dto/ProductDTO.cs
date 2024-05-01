using Application.Category.Dto;
using Domain.Utils.Enums;
using System.Text.Json.Serialization;
using Entities = Domain.Entities;
using ValueObjects = Domain.Product.ValueObjects;

namespace Application.Product.Dto
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public List<CategoryDTO>? Categories { get; set; }
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
                Categories = new List<Entities.Category>()
            };
        }

        public static ProductDTO MapToDTO(Entities.Product product)
        {
            var productDto = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.Value
            };

            var categories = new List<CategoryDTO>();

            if (product.Categories != null && product.Categories.Count > 0)
            {
                foreach (var category in product.Categories)
                {
                    categories.Add(CategoryDTO.MapToDTO(category));
                }

                productDto.Categories = categories;
            }

            return productDto;


        }
    }
}
