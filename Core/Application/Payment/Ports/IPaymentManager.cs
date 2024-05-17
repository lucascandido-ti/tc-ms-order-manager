
using Application.Payment.Requests;
using Application.Payment.Responses;

namespace Application.Payment.Ports
{
    public interface IPaymentManager
    {
        Task<PaymentResponse> ProcessPayment(ProcessPaymentRequest request);
    }
}
