
namespace Domain.Queue.Ports
{
    public interface IQueueConsumer
    {
        Task ExecuteAsync(CancellationToken stoppingToken);
    }
}
