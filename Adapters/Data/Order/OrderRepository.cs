using Domain.Order.Ports;

namespace Data.Order
{
    public class OrderRepository : IOrderRepository
    {
        public Task<Domain.Entities.Order> CreateOrder(Domain.Entities.Order order)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Entities.Order> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Domain.Entities.Order>> List()
        {
            throw new NotImplementedException();
        }
    }
}
