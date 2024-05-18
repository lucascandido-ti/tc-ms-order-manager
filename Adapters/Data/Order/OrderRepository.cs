using Entities = Domain.Entities;
using Domain.Order.Ports;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Order.Enums;

namespace Data.Order
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DataDbContext _dbContext;

        public OrderRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Entities.Order> CreateOrder(Entities.Order order)
        {
            foreach (var product in order.Products)
            {
                _dbContext.Entry(product).State = EntityState.Unchanged;
            }

            _dbContext.Entry(order.Customer).State = EntityState.Unchanged;

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task<Entities.Order> Get(int orderId)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Products)
                .Where(o => o.Id == orderId)
                .FirstOrDefaultAsync();

            return order;
        }

        public async Task<List<Entities.Order>> List()
        {
            var orders = await _dbContext.Orders
                .Include(o => o.Customer)
                .Include(o => o.Products)
                .ToListAsync();

            return orders;
        }

        public async Task<Entities.Order> UpdateStatus(Entities.Order order, OrderStatus status)
        {
            var data = await _dbContext.Orders.Where(o => o.Id == order.Id).FirstOrDefaultAsync();
            data.Status = status;
            data.LastUpdatedAt = DateTime.UtcNow;

            _dbContext.Orders.Update(data);

            await _dbContext.SaveChangesAsync();

            var getFullOrder = await Get(data.Id);

            return getFullOrder;
        }

    }
}
