using Domain.Order.Enums;

namespace Domain.Order.Ports
{
    public interface IOrderRepository
    {
        Task<Entities.Order> CreateOrder(Entities.Order order);
        Task<Entities.Order> UpdateStatus(Entities.Order order, OrderStatus status);
        Task<Entities.Order> Get(int id);
        Task<List<Entities.Order>> List();
    }
}
