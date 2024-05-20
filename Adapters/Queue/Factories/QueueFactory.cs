// NOSONAR

using Microsoft.Extensions.Configuration;
using Queue.Ports;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Queue.Factories
{
    public class QueueFactory: IQueueFactory
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly string _queueName;
        private readonly IModel _channel;

        public QueueFactory(IConfiguration configuration, string queueName, string connection)
        {
            var rabbitMQConfig = configuration.GetSection("RabbitMQ");
            var hostName = rabbitMQConfig.GetValue<string>("HostName");
            var port = rabbitMQConfig.GetValue<int>("Port");
            var username = rabbitMQConfig.GetValue<string>("UserName");
            var password = rabbitMQConfig.GetValue<string>("Password");

            var connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = username,
                Password = password
            };

            _queueName = queueName;
            _connection = connectionFactory.CreateConnection(connection);
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        public ConnectionFactory getConnection()
        {
            return _connectionFactory;
        }

        public IModel getChannel()
        {
            return _channel;
        }

        public Task ConsumeAsync(Func<string, Task> processMessageAsync)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await processMessageAsync(message);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
