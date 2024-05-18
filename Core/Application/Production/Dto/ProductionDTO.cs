using Domain.Order.Enums;
using Domain.Utils.Enums;

namespace Application.Production.Dto
{
    public class ProductionOrder
    {
        public string id { get; set; }
        public int orderId { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public int Invoice { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethodString PaymentMethod { get; set; }
        public string customerId { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }
    public class ProductionDTO
    {
        public string id { get; set; }
        public string status { get; set; }
        public ProductionOrder order { get; set; }
    }
}
