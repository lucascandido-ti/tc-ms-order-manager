
using Application.Payment.Dto;
using Domain.Utils;

namespace Application.Payment.Responses
{
    public class PaymentResponse: Response
    {
        public PaymentDTO Data { get; set; }
    }
}
