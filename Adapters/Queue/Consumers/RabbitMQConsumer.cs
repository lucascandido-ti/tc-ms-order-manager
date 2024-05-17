
using Application.Payment.Dto;
using Domain.Queue.Ports;
using Newtonsoft.Json;
using Queue.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Queue.Consumers
{
    public class RabbitMQConsumer : IQueueConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName = "payment-service-queue";

        public RabbitMQConsumer()
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = "localhost",
            };

            _connection = connectionFactory.CreateConnection("payment-service-consumer");
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(_queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (sender, eventArgs) =>
            {
                var body = eventArgs.Body.Span;
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine("Queue:{0}, Received Message: \"{1}\"", _queueName, message);
                //  _channel.BasicAck(eventArgs.DeliveryTag, false);
            };
            _channel.BasicConsume(_queueName, false, consumer);
        }

        public Task ExecuteAsync(CancellationToken stoppingToken)
        {   
            return Task.CompletedTask;
        }
    }
}
