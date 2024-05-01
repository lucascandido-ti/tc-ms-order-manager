using Application.Customer.Dto;
using Application.Customer.Requests;
using Application.Customer.Responses;

namespace Application.Customer.Ports
{
    public interface ICustomerManager
    {
        Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request);
        Task<CustomerResponse> GetCustomer(int id);
        Task<List<CustomerDTO>> GetCustomers();
    }
}
