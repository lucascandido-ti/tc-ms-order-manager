
using Application.Customer.Ports;
using Application.Customer.Requests;
using Application.Customer.Responses;
using MediatR;

namespace Application.Customer.Commands
{
    public class CreateCustomerCommandHandler: IRequestHandler<CreateCustomerCommand, CustomerResponse>
    {
        private readonly ICustomerManager _customerManager;

        public CreateCustomerCommandHandler(ICustomerManager customerManager)
        {
            _customerManager = customerManager;
        }

        public Task<CustomerResponse> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var req = new CreateCustomerRequest
            {
                Data = request.customerDTO
            };

            return _customerManager.CreateCustomer(req);
        }
    }
}
