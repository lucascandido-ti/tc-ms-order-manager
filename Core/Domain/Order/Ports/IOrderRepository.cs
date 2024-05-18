namespace Domain.Order.Ports
{
    public interface IOrderRepository
    {
        Task<Entities.Order> CreateOrder(Entities.Order order);
        Task<Entities.Order> SendOrderToProduction(Entities.Order order);
        Task<Entities.Order> Get(int id);
        Task<List<Entities.Order>> List();
    }
}
