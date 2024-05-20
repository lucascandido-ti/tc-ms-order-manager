using Entities = Domain.Entities;
using Moq;

using Domain.Utils;
using Domain.Order.Enums;
using Domain.Order.Ports;
using Domain.Utils.Enums;
using Domain.Queue.Ports;

using Application.Order;
using Application.Order.Dto;
using Application.Order.Requests;
using Application.Order.Queries;
using Application.Product.Dto;
using Application.Customer.Dto;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Queue.Ports;

namespace ApplicationTests.Order
{
    class FakeRepoOrder : IOrderRepository
    {
        
        public OrderDTO GetMoqOrder()
        {
            var customer = new CustomerDTO
            {
                Id = 111,
                Name = "Fulano",
                Email = "email@email.com",
                Cpf = "549.714.950-29"
            };

            var products = new List<ProductDTO>
            {
                new ProductDTO
                {
                    Id = 111,
                    Name = "Milkshake",
                    Description = "Sobremesa e Bebidas",
                    Price = 12.99m,
                    Currency = 0
                }
            };

            var orderDTO = new OrderDTO
            {
                Id = 111,
                Price = 129.99m,
                Currency = AcceptedCurrencies.Real,
                Invoice = 0,
                Status = OrderStatus.PENDING,
                PaymentMethod = PaymentMethod.QRCode,
                Customer = customer,
                Products = products
            };

            return orderDTO;
        }
        public Task<Entities.Order> CreateOrder(Entities.Order order)
        {

            var orderDTO = GetMoqOrder();

            var orderEntity = OrderDTO.MapToEntity(orderDTO);

            return Task.FromResult(orderEntity);
        }

        public Task<Entities.Order> Get(int id)
        {
            var orderDTO = GetMoqOrder();

            var orderEntity = OrderDTO.MapToEntity(orderDTO);

            return Task.FromResult(orderEntity);
        }

        public Task<List<Entities.Order>> List()
        {
            var orderDTO = GetMoqOrder();

            var orderEntity = OrderDTO.MapToEntity(orderDTO);

            return Task.FromResult(new List<Entities.Order> { orderEntity });
        }

        public Task<Entities.Order> UpdateStatus(Entities.Order order, OrderStatus status)
        {
            throw new NotImplementedException();
        }
    }
    public class Tests
    {
        
        OrderManager orderManager;

        public async Task<Task> MockProductionMessageAsync(string message) { return Task.CompletedTask; }

        [SetUp]
        public void Setup() { }

        [Test]
        public async Task ShouldBeAbleToValidateCreateOrder()
        {
            var orderDTO = new FakeRepoOrder().GetMoqOrder();

            var request = new CreateOrderRequest()
            {
                Data = orderDTO
            };

            var fakeRepoOrder = new Mock<IOrderRepository>();
            var fakeRepoQueue = new Mock<IQueueRepository>();

            var expectEntity = OrderDTO.MapToEntity(orderDTO);
            expectEntity.Id = 111;

            fakeRepoOrder.Setup(x => x.CreateOrder(It.IsAny<Entities.Order>()))
                .Returns(Task.FromResult(expectEntity));


            fakeRepoQueue.Setup(x => x.Publish(It.IsAny<Entities.Order>(), "create-new-order-test", "order-service-queue-test"));

            orderManager = new OrderManager(fakeRepoOrder.Object, fakeRepoQueue.Object);

            var res = await orderManager.CreateOrder(request);

            Assert.IsNotNull(res);
            Assert.True(res.Success);
        }

        [Test]
        public async Task ShouldBeAbleToReceiveAPaymentQueueEventToSendOrderToProduction()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            var serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            var serviceScopeMock = new Mock<IServiceScope>();
            var mediatorMock = new Mock<IMediator>();

            serviceProviderMock.Setup(sp => sp.GetService(typeof(IMediator))).Returns(mediatorMock.Object);
            serviceScopeFactoryMock.Setup(ssf => ssf.CreateScope()).Returns(serviceScopeMock.Object);
            serviceScopeMock.Setup(ss => ss.ServiceProvider).Returns(serviceProviderMock.Object);

            serviceProviderMock.Setup(sp => sp.GetService(typeof(IServiceScopeFactory))).Returns(serviceScopeFactoryMock.Object);

            var configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            configurationMock.SetupGet(c => c["RabbitMQ:HostName"]).Returns("localhost");
            configurationMock.SetupGet(c => c["RabbitMQ:Port"]).Returns("5672");
            configurationMock.SetupGet(c => c["RabbitMQ:UserName"]).Returns("guest");
            configurationMock.SetupGet(c => c["RabbitMQ:Password"]).Returns("guest");

            var factoryMock = new Mock<IQueueFactory>();
            factoryMock.Setup(f => f.ConsumeAsync(MockProductionMessageAsync));

            var consumer = new Mock<IPaymentCustomer>();

            var message = "{\"pattern\":\"processed-payment\",\"data\":{\"id\":\"1\",\"amount\":10.0}}";

            consumer.Setup(f => f.ProcessPaymentMessageAsync(message)).Returns(Task.CompletedTask);
            
        }

        [Test]
        public void ShouldBeAbleToConsumeEvents()
        {
            // Arrange
            var configurationMock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            configurationMock.SetupGet(c => c["RabbitMQ:HostName"]).Returns("localhost");
            configurationMock.SetupGet(c => c["RabbitMQ:Port"]).Returns("5672");
            configurationMock.SetupGet(c => c["RabbitMQ:UserName"]).Returns("guest");
            configurationMock.SetupGet(c => c["RabbitMQ:Password"]).Returns("guest");

            var factoryMock = new Mock<IQueueFactory>();
            factoryMock.Setup(f => f.getConnection());
            factoryMock.Setup(f => f.getChannel());
            factoryMock.Setup(f => f.ConsumeAsync(MockProductionMessageAsync));

        }

        [Test]
        public async Task ShouldReturnOrderNotFoundWhenOrderDoesntExist()
        {
            var fakeRepoOrder = new Mock<IOrderRepository>();
            var fakeRepoQueue = new Mock<IQueueRepository>();

            var fakeOrder = new FakeRepoOrder().GetMoqOrder();

            fakeRepoOrder.Setup(x => x.Get(333)).Returns(Task.FromResult<Entities.Order?>(null));

            orderManager = new OrderManager(fakeRepoOrder.Object, fakeRepoQueue.Object);

            var query = new GetOrderQuery
            {
                Id = 333
            };

            var res = await orderManager.GetOrder(query);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.ORDER_NOT_FOUND);
            Assert.AreEqual(res.Message, "No order record was found with the given Id");
        }

        [Test]
        public async Task ShouldReturnOrderSuccess()
        {
            var fakeRepoOrder = new Mock<IOrderRepository>();
            var fakeRepoQueue = new Mock<IQueueRepository>();

            var fakeOrderDTO = new FakeRepoOrder().GetMoqOrder();

            var fakeOrderEntity = OrderDTO.MapToEntity(fakeOrderDTO);

            fakeRepoOrder.Setup(x => x.Get(111)).Returns(Task.FromResult((Entities.Order?)fakeOrderEntity));

            orderManager = new OrderManager(fakeRepoOrder.Object, fakeRepoQueue.Object);

            var query = new GetOrderQuery
            {
                Id = 111
            };

            var res = await orderManager.GetOrder(query);

            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Data.Id, fakeOrderEntity.Id);
            Assert.AreEqual(res.Data.Price, fakeOrderEntity.Price.Value);
            Assert.AreEqual(res.Data.Products.Count, fakeOrderEntity.Products.Count);
            Assert.AreEqual(res.Data.Customer.Name, fakeOrderEntity.Customer.Name);

        }
    }
}
