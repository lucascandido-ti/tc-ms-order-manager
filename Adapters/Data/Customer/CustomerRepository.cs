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

        public async Task<Entities.Customer> CreateCustomer(Entities.Customer customer)
        {
            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();
            return customer;
        }

        public async Task<Entities.Customer> Get(int id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);
            return customer;
        }

        public Task<List<Entities.Customer>> List()
        {
            throw new NotImplementedException();
        }
    }
}
