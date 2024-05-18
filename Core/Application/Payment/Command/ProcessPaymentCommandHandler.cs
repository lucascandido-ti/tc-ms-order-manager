
using Application.Payment.Ports;
using Application.Payment.Requests;
using Application.Payment.Responses;
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

        public Task<PaymentResponse> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            var req = new ProcessPaymentRequest
            {
                Data = request.paymentDTO
            };

            return _paymentManager.ProcessPayment(req);
        }
    }
}
