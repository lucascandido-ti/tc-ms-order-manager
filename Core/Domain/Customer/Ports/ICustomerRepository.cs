namespace Domain.Customer.Ports
{
    public interface ICustomerRepository
    {
        Task<Entities.Customer> Get(int id);
        Task<List<Entities.Customer>> List();
        Task<Entities.Customer> CreateCustomer(Entities.Customer customer);
    }
}
