using Application.Order.Dto;

namespace Application.Order.Events
{
    public class CreateOrderEvent
    {
        public OrderDTO Order { get; set; }
    }
}
