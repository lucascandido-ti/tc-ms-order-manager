namespace Domain.Order.Ports
{
    public interface IOrderRepository
    {
        Task<Entities.Order> Get(int id);
        Task<List<Entities.Order>> List();
    }
}
