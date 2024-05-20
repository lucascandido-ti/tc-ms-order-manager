// NOSONAR

using Application.Payment.Command;
using Application.Payment.Dto;
using Domain.Queue.Ports;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Queue.Factories;
using Queue.Utils;

namespace Queue.Consumers.Payment
{
    public class PaymentConsumer : BackgroundService, IPaymentCustomer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly QueueFactory _queueInstanse;


        public PaymentConsumer(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _queueInstanse = new QueueFactory(configuration, "payment-service-queue", "payment-consumer");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _queueInstanse.ConsumeAsync(ProcessPaymentMessageAsync);
        }


        public async Task ProcessPaymentMessageAsync(string message)
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
