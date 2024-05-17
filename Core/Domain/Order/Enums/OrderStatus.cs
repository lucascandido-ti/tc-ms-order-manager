using Domain.Utils;

namespace Domain.Order.Enums
{
    public enum OrderStatus
    {
        [StringValue("Pending")]
        PENDING,
        
        [StringValue("Received")]
        RECEIVED,

        [StringValue("InPreparation")]
        IN_PREPARATION,

        [StringValue("Concluded")]
        CONCLUDED,

        [StringValue("Finished")]
        FINISHED
    }
}
