using Domain.Order.Enums;
using Domain.Order.Exceptions;
using Domain.Order.Ports;
using Domain.Utils.ValueObjects;

namespace Domain.Entities
{
    public class Order
    {
        public Order()
        {
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
        }
        public int Id { get; set; }
        public Price Price { get; set; }
        public int Invoice { get; set; }
        public OrderStatus status { get; set; }
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

        public async Task Save(IOrderRepository orderRepository)
        {
            this.ValidadeState();

            if(this.Id == 0)
            {
                var result = await orderRepository.CreateOrder(this);
                this.Id = result.Id;
            }
        }
    }
}
