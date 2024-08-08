using Application.Customer.Responses;
using MediatR;

namespace Application.Customer.Commands
{
    public class DeletePersonalDataCommand: IRequest<CustomerResponse>
    {
        public int Id { get; set; }
    }
}
