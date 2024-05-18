
using Application.Order.Command;
using Application.Order.Ports;
using Application.Order.Queries;
using Application.Order.Requests;
using Application.Payment.Dto;
using Application.Payment.Ports;
using Application.Payment.Requests;
using Application.Payment.Responses;
using Domain.Queue.Ports;
using Domain.Utils;
using Domain.Utils.Enums;
using MediatR;

namespace Application.Payment
{
    public class PaymentManager : IPaymentManager
    {
        private readonly IMediator _mediator;

        public PaymentManager(IMediator mediator)
        {
            _mediator = mediator;

        }

        public async Task<PaymentResponse> ProcessPayment(ProcessPaymentRequest request)
        {
            
            var payment = request.Data;

            if (payment.paymentStatus != PaymentSatus.CONCLUDED)
            {
                return new PaymentResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PAYMENT_NOT_PROCESSED,
                    Message = "Payment cannot be processed"
                };
            }

            var command = new SendOrderToProductionCommand
            {
                orderId = payment.orderId
            };

            await _mediator.Send(command);

            return new PaymentResponse
            {
                Success = true,
                Message = "Payment Processed"
            };
            
        }
    }
}
