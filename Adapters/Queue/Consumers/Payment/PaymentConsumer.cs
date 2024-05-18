
using Application.Payment.Command;
using Application.Payment.Dto;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Queue.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Queue.Consumers.Payment
{
    public class PaymentConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string QueueName = "payment-service-queue";


        public PaymentConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory() { HostName = "localhost" }; // Ajuste as configurações conforme necessário
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await ProcessPaymentMessageAsync(message);
            };
            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }


        private async Task ProcessPaymentMessageAsync(string message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                try
                {
                    var queueData = JsonParser.ParseMessage<PaymentDTO>(message);
                    if (queueData != null && queueData.pattern == "processed-payment")
                    {
                        var command = new ProcessPaymentCommand
                        {
                            paymentDTO = queueData.data
                        };

                        var res = await mediator.Send(command);

                        Console.WriteLine("ProcessPaymentMessageAsync | Status: {0} | Message: \"{1}\"", res.Success, res.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ProcessPaymentMessageAsync Erro: {0}", ex.Message);
                }

            }
        }
    }
}
