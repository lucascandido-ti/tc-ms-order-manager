using Domain.Queue.Ports;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Queue.Repositories
{
    public class RabbitMQRepository : IQueueRepository
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string _exchange = "order-service";

        public RabbitMQRepository(IConfiguration configuration)
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

            _connection = connectionFactory.CreateConnection("order-service-publisher");

            _channel = _connection.CreateModel();

        }

        public void Publish(object data, string routingKey, string queueName)
        {
            var queue = queueName + "." + routingKey;
            var exchange = _exchange + "." + routingKey;

            _channel.ExchangeDeclare(exchange, ExchangeType.Topic, durable: true);
            _channel.QueueDeclare(queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue, exchange, routingKey);

            var type = data.GetType();

            object patternData = new 
            {
                pattern = routingKey,
                data = data
            };

            var payload = JsonConvert.SerializeObject(patternData);

            var byteArray = Encoding.UTF8.GetBytes(payload);

            Console.WriteLine($"{type.Name} Published");

            _channel.BasicPublish(exchange, routingKey, null, byteArray);
        }
    }
}
