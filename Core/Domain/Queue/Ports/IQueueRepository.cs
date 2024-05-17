
namespace Domain.Queue.Ports
{
    public interface IQueueRepository
    {
        void Publish(object data, string routingKey, string queueName);
    }
}
