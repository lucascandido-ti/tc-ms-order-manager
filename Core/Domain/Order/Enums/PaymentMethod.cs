using Domain.Utils;

namespace Domain.Order.Enums
{
    public enum PaymentMethod
    {
        QRCode,
        CreditCard,
        DebitCard
    }

    public enum PaymentMethodString
    {
        [StringValue("QRCode")]
        QRCode,

        [StringValue("CreditCard")]
        CreditCard,

        [StringValue("DebitCard")]
        DebitCard
    }
}
