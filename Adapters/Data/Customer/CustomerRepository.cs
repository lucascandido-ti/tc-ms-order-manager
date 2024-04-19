using Domain.Customer.Ports;

namespace Data.Customer
{
    public class CustomerRepository : ICustomerRepository
    {
        public Task<Domain.Entities.Customer> CreateBooking(Domain.Entities.Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.Entities.Customer> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Domain.Entities.Customer>> List()
        {
            throw new NotImplementedException();
        }
    }
}
