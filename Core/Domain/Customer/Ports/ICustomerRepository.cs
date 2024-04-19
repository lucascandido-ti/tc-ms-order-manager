namespace Domain.Customer.Ports
{
    public interface ICustomerRepository
    {
        Task<Entities.Customer> Get(int id);
        Task<List<Entities.Customer>> List();
        Task<Entities.Customer> CreateBooking(Entities.Customer customer);
    }
}
