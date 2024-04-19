using Application.Customer.Dto;
using Application.Customer.Responses;
using MediatR;

namespace Application.Customer.Commands
{
    public class CreateCustomerCommand: IRequest<CustomerResponse>
    {
        public CustomerDTO customerDTO { get; set; }
    }
}
