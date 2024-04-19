using Domain.Order.Enums;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public float Price { get; set; }
        public int Invoice { get; set; }
        public OrderStatus status { get; set; }
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
