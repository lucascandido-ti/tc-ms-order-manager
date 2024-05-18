namespace Domain.Utils.Enums
{
    public enum AcceptedCurrencies
    {
        Real,
        Dollar,
        Euro
    }

    public enum PaymentSatus
    {
        [StringValue("Pending")]
        PENDING,

        [StringValue("Expired")]
        EXPIRED,

        [StringValue("Concluded")]
        CONCLUDED
    }

    public enum ProductionStatus
    {
        [StringValue("Received")]
        RECEIVED,

        [StringValue("In Preparation")]
        IN_PREPARATION,

        [StringValue("Concluded")]
        CONCLUDED
    }
    
}
