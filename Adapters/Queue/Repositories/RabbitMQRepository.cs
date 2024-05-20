// NOSONAR

using Domain.Queue.Ports;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Queue.Factories;
using RabbitMQ.Client;
using System.Text;

namespace Queue.Repositories
{
    public class RabbitMQRepository : IQueueRepository
    {
        private readonly IModel _channel;
        private const string _exchange = "order-service";
        private readonly QueueFactory _queueInstanse;

        public RabbitMQRepository(IConfiguration configuration)
        {
            _queueInstanse = new QueueFactory(configuration, "order-service-queue", "order-service-producer");
            _channel = _queueInstanse.getChannel();
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
