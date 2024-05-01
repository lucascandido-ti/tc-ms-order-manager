using Entities = Domain.Entities;
using Domain.Order.Enums;
using Application.Product.Dto;
using Domain.Utils.Enums;
using Domain.Utils.ValueObjects;

namespace Application.Order.Dto
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public AcceptedCurrencies Currency { get; set; }
        public int Invoice { get; set; }
        public OrderStatus status { get; set; }
        public List<ProductDTO> Products { get; set; }

        public static Entities.Order MapToEntity(OrderDTO dto)
        {
            var productEntity = new List<Entities.Product>();

            foreach (var product in dto.Products)
            {
                productEntity.Add(ProductDTO.MapToEntity(product));
            }

            return new Entities.Order
            {
                Id = dto.Id,
                Invoice = dto.Invoice,
                Price = new Price { Currency = dto.Currency, Value = dto.Price },
                status = dto.status,
                Products = productEntity
            };
        }

        public static OrderDTO MapToDTO(Entities.Order order)
        {
            var productsDto = new List<ProductDTO>();

            foreach(var product in order.Products)
            {
                productsDto.Add(ProductDTO.MapToDTO(product));
            }

            var orderDto = new OrderDTO
            {
                Id = order.Id,
                Invoice = order.Invoice,
                Price = order.Price.Value,
                Currency = order.Price.Currency,
                Products = productsDto,
                status = order.status
            };

            return orderDto;
        }
    }
}
