namespace Domain.Utils
{
    public enum ErrorCodes
    {
        // Customer
        CUSTOMER_NOT_FOUND = 1,
        CUSTOMER_NAME_REQUIRED,
        CUSTOMER_EMAIL_REQUIRED,
        CUSTOMER_CPF_REQUIRED,

        // Category
        CATEGORY_NOT_FOUND = 100,
        CATEGORY_NAME_REQUIRED,
        CATEGORY_DESCRIPTION_REQUIRED,


        // Product
        PRODUCT_NOT_FOUND = 200,
        PRODUCT_NAME_REQUIRED,
        PRODUCT_DESCRIPTION_REQUIRED,
        PRODUCT_PRICE_REQUIRED,
        PRODUCT_CATEGORIES_REQUIRED,
    }
    public abstract class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public ErrorCodes ErrorCode { get; set; }
    }
}
