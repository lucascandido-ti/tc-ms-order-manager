
using Domain.Order.Enums;
using Domain.Utils.Enums;

namespace Application.Payment.Dto
{
    public class PaymentDTO
    {
        public int id { get; set; }
        public PaymentMethod paymentMethod { get; set; }
        public PaymentSatus paymentStatus { get; set; }
        public int orderId { get; set; }
        public int customerId { get; set; }
    }
}
