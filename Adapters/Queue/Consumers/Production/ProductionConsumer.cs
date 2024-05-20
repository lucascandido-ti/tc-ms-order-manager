using Application.Production.Commands;
using Application.Production.Commands.StartProduction;
using Application.Production.Dto;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Queue.Factories;
using Queue.Utils;

namespace Queue.Consumers.Payment
{
    public class ProductionConsumer : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly QueueFactory _queueInstanse;


        public ProductionConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _queueInstanse = new QueueFactory(configuration, "production-service-queue", "production-consumer");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _queueInstanse.ConsumeAsync(ProductionMessageAsync);
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
