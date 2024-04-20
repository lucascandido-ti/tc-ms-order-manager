using Application.Customer.Dto;
using Application.Customer.Ports;
using Application.Customer.Responses;
using Domain.Customer.Ports;
using MediatR;

namespace Application.Customer.Queries
{
    public class GetCustomerQueryHandler: IRequestHandler<GetCustomerQuery, CustomerResponse>
    {
        private readonly ICustomerRepository _customerRepository;

        public GetCustomerQueryHandler(ICustomerRepository customerRepository)
        {
            this._customerRepository = customerRepository;
        }

        public async Task<CustomerResponse> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.Get(request.Id);

            var customerDto = CustomerDTO.MapToDTO(customer);

            return new CustomerResponse
            {
                Success = true,
                Data = customerDto,
            };
        }
    }
}
