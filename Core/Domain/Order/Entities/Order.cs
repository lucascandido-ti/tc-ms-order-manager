using Domain.Order.Enums;
using Domain.Order.Exceptions;
using Domain.Order.Ports;
using Domain.Utils.Enums;
using Domain.Utils.ValueObjects;

namespace Domain.Entities
{
    public class Order
    {
        public Order()
        {
            Status = OrderStatus.PENDING;
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public Price Price { get; set; }
        public int Invoice { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public ICollection<Product> Products { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        private void ValidadeState()
        {
            if(this.Products == null || this.Products.Count() == 0)
            {
                throw new OrderProductsRequiredExceptions();
            }
        }

        public decimal CalculateProducts(ICollection<Product> products)
        {
            var price = 0m;

            foreach(var product in products)
            {
                price += product.Price.Value;
            }

            return price;
        }

        public async Task Save(IOrderRepository orderRepository)
        {
            this.ValidadeState();

            if(this.Id == 0)
            {
                this.Price = new Price { Currency = AcceptedCurrencies.Real, Value = CalculateProducts(this.Products) };
                this.Invoice = 0;
                
                var result = await orderRepository.CreateOrder(this);
                this.Id = result.Id;
            }
        }
    }
}
