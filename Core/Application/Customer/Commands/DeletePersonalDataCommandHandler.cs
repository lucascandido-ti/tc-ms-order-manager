using Application.Customer.Ports;
using Application.Customer.Requests;
using Application.Customer.Responses;
using MediatR;

namespace Application.Customer.Commands
{
    public class DeletePersonalDataCommandHandler : IRequestHandler<DeletePersonalDataCommand, CustomerResponse>
    {

        private readonly ICustomerManager _customerManager;

        public DeletePersonalDataCommandHandler(ICustomerManager customerManager)
        {
            _customerManager = customerManager;
        }
        public Task<CustomerResponse> Handle(DeletePersonalDataCommand request, CancellationToken cancellationToken)
        {
            return _customerManager.DeleteCustomer(request.Id);
        }
    }
}
