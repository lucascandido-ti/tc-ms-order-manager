namespace Domain.Queue.Ports
{
    public interface IPaymentCustomer
    {
        Task ProcessPaymentMessageAsync(string message);
    }
}
