using Application.Order.Responses;
using Application.Payment.Dto;
using Application.Payment.Responses;
using MediatR;

namespace Application.Payment.Command
{
    public class ProcessPaymentCommand : IRequest<PaymentResponse>
    {
        public PaymentDTO paymentDTO { get; set; }
    }
}
