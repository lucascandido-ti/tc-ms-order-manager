using Application.Order.Ports;
using Application.Order.Responses;
using Application.Payment.Ports;
using Application.Payment.Requests;
using Application.Payment.Responses;
using Domain.Utils.Enums;
using MediatR;

namespace Application.Payment.Command
{

    public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, PaymentResponse>
    {

        private readonly IPaymentManager _paymentManager;

        public ProcessPaymentCommandHandler(IPaymentManager paymentManager)
        {
            _paymentManager = paymentManager;
        }

        public async Task<PaymentResponse> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            var payload = new ProcessPaymentRequest
            {
                Data = request.paymentDTO
            };

            return await _paymentManager.ProcessPayment(payload);
        }
    }
}
