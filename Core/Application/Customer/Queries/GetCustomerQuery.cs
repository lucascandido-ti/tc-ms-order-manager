using Application.Customer.Responses;
using MediatR;

namespace Application.Customer.Queries
{
    public class GetCustomerQuery: IRequest<CustomerResponse>
    {
        public int Id { get; set; }
    }
}
