namespace Domain.Utils
{
    public enum ErrorCodes
    {
        // Customer
        CUSTOMER_NOT_FOUND = 1,
        CUSTOMER_NAME_REQUIRED,
        CUSTOMER_EMAIL_REQUIRED,
        CUSTOMER_CPF_REQUIRED
    }
    public abstract class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErrorCodes ErrorCode { get; set; }
    }
}
