
using Application.Customer.Dto;
using Application.Customer.Ports;
using Application.Customer.Requests;
using Application.Customer.Responses;
using Domain.Customer.Exceptions;
using Domain.Customer.Ports;
using Domain.Utils;

namespace Application.Customer
{
    public class CustomerManager : ICustomerManager
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerManager(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<CustomerResponse> CreateCustomer(CreateCustomerRequest request)
        {
            try
            {
                var customer = CustomerDTO.MapToEntity(request.Data);

                await customer.Save(_customerRepository);

                request.Data.Id = customer.Id;

                return new CustomerResponse
                {
                    Success = true,
                    Data = request.Data,
                };
            }
            catch (NameRequiredException)
            {
                return new CustomerResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.CUSTOMER_NAME_REQUIRED,
                    Message = "Name is a required information"
                };
            }
            catch (EmailRequiredException)
            {
                return new CustomerResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.CUSTOMER_EMAIL_REQUIRED,
                    Message = "Email is a required information"
                };
            }
            catch (CpfRequiredException)
            {
                return new CustomerResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.CUSTOMER_CPF_REQUIRED,
                    Message = "CPF is a required information"
                };
            }
        }

        public Task<CustomerResponse> GetCustomer(int id)
        {
            throw new NotImplementedException();
        }
    }
}
