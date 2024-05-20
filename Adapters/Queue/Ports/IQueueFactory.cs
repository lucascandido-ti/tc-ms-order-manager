using RabbitMQ.Client;

namespace Queue.Ports
{
    public interface IQueueFactory
    {
        public ConnectionFactory getConnection();
        public IModel getChannel();
        public Task ConsumeAsync(Func<string, Task> processMessageAsync);
    }
}
