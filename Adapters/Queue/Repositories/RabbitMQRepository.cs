using Domain.Queue.Ports;
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

        public RabbitMQRepository()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
            };

            _connection = connectionFactory.CreateConnection("order-service-publisher");

            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_exchange, ExchangeType.Topic, durable: true);

            
        }

        public void Publish(object data, string routingKey, string queueName)
        {
            _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queueName, _exchange, routingKey);

            var type = data.GetType();

            object patternData = new 
            {
                pattern = routingKey,
                data = data
            };

            var payload = JsonConvert.SerializeObject(patternData);

            var byteArray = Encoding.UTF8.GetBytes(payload);

            Console.WriteLine($"{type.Name} Published");

            _channel.BasicPublish(_exchange, routingKey, null, byteArray);
        }
    }
}
