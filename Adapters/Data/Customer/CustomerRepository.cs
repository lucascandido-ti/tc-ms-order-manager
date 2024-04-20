using Domain.Customer.Ports;
using Entities = Domain.Entities;

namespace Data.Customer
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly DataDbContext _dbContext;

        public CustomerRepository(DataDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Entities.Customer> CreateBooking(Entities.Customer customer)
        {
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public Task<Entities.Customer> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Entities.Customer>> List()
        {
            throw new NotImplementedException();
        }
    }
}
