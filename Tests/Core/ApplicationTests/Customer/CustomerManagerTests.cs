using Entities = Domain.Entities;

using Application.Customer;
using Application.Customer.Dto;
using Application.Customer.Requests;
using Domain.Customer.Ports;
using Moq;

namespace ApplicationTests.Customer
{
    class FakeRepo : ICustomerRepository
    {
        public Task<Entities.Customer> CreateCustomer(Entities.Customer customer)
        {
            var customerDTO = new CustomerDTO
            {
                Id = 111,
                Name = "Fulano",
                Email = "Silva",
                Cpf = "123.456.789-00"
            };

            var customerEntities = CustomerDTO.MapToEntity(customerDTO);

            return Task.FromResult(customerEntities);
        }

        public Task<Entities.Customer> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Entities.Customer>> List()
        {
            throw new NotImplementedException();
        }
    }
    public class Tests
    {

        CustomerManager customerManager;

        [SetUp]
        public void Setup(){}

        [Test]
        public async Task ShouldBeAbleToValidateCreateCustomer()
        {
            var customerDTO = new CustomerDTO
            {
                Name = "Fulano",
                Email = "Silva",
                Cpf = "123.456.789-00"
            };

            var request = new CreateCustomerRequest()
            {
                Data = customerDTO
            };

            var fakeRepo = new Mock<ICustomerRepository>();


            var expectEntity = CustomerDTO.MapToEntity(customerDTO);
            expectEntity.Id = 111;

            fakeRepo.Setup(x => x.CreateCustomer(It.IsAny<Entities.Customer>()))
                    .Returns(Task.FromResult(expectEntity));

            customerManager = new CustomerManager(fakeRepo.Object);

            var res = await customerManager.CreateCustomer(request);
            Assert.IsNotNull(res);
            Assert.True(res.Success);
        }
    }
}
