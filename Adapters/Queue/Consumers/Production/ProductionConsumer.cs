

using Application.Production.Commands;
using Application.Production.Commands.StartProduction;
using Application.Production.Dto;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Queue.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Queue.Consumers.Payment
{
    public class ProductionConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string QueueName = "production-service-queue";


        public ProductionConsumer(IServiceProvider serviceProvider)
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
                await ProductionMessageAsync(message);
            };
            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);
            return Task.CompletedTask;
        }


        private async Task ProductionMessageAsync(string message)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                try
                {
                    var queueData = JsonParser.ParseMessage<ProductionDTO>(message);
                    if (queueData != null)
                    {
                        switch (queueData.pattern)
                        {
                            case "start-production":
                                await mediator.Send(new StartProductionOrderCommand
                                {
                                    productionDTO = queueData.data
                                });
                                break;
                            case "concluded-production":
                                await mediator.Send(new ConcludedProductionCommand
                                {
                                    productionDTO = queueData.data
                                });
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ProductionMessageAsync Erro: {0}", ex.Message);
                }

            }
        }
    }
}
