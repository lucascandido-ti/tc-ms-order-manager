namespace Domain.Queue.Ports
{
    public interface IProductionConsumer
    {
        Task ProductionMessageAsync(string message);
    }
}
