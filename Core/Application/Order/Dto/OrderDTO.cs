using Entities = Domain.Entities;
using Domain.Order.Enums;
using Application.Product.Dto;
using Domain.Utils.Enums;
using Domain.Utils.ValueObjects;
using Application.Customer.Dto;

namespace Application.Order.Dto
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public AcceptedCurrencies Currency { get; set; }
        public int Invoice { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public CustomerDTO Customer { get; set; }
        public List<ProductDTO> Products { get; set; }

        public static Entities.Order MapToEntity(OrderDTO dto)
        {
            var productEntity = new List<Entities.Product>();

            foreach (var product in dto.Products)
            {
                productEntity.Add(ProductDTO.MapToEntity(product));
            }

            var customer = CustomerDTO.MapToEntity(dto.Customer);

            return new Entities.Order
            {
                Id = dto.Id,
                Invoice = dto.Invoice,
                Price = new Price { Currency = dto.Currency, Value = dto.Price },
                Status = dto.Status,
                PaymentMethod = dto.PaymentMethod,
                Products = productEntity,
                Customer = customer,
                CustomerId = customer.Id
            };
        }

        public static OrderDTO MapToDTO(Entities.Order order)
        {
            var productsDto = new List<ProductDTO>();

            foreach(var product in order.Products)
            {
                productsDto.Add(ProductDTO.MapToDTO(product));
            }

            var customerDto = CustomerDTO.MapToDTO(order.Customer);

            var orderDto = new OrderDTO
            {
                Id = order.Id,
                Invoice = order.Invoice,
                Price = order.Price.Value,
                Currency = order.Price.Currency,
                Products = productsDto,
                Status = order.Status,
                PaymentMethod = order.PaymentMethod,
                Customer = customerDto
            };

            return orderDto;
        }
    }
}
