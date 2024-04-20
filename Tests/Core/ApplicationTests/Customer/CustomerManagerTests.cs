using Entities = Domain.Entities;

using Application.Customer;
using Application.Customer.Dto;
using Application.Customer.Requests;
using Domain.Customer.Ports;
using Moq;
using Domain.Utils;

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
                Email = "email@email.com",
                Cpf = "549.714.950-29"
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
                Email = "email@email.com",
                Cpf = "549.714.950-29"
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


        [TestCase("invalid")]
        public async Task ShouldReturnInvalidEmailPassed(string email)
        {
            var customerDTO = new CustomerDTO
            {
                Name = "Fulano",
                Email = email,
                Cpf = "549.714.950-29"
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
            Assert.False(res.Success);

            Assert.AreEqual(res.ErrorCode, ErrorCodes.CUSTOMER_EMAIL_REQUIRED);
            Assert.AreEqual(res.Message, "Email is a required information");
        }

        [TestCase("123.456.789-00")]
        public async Task ShouldReturnInvalidCPFPassed(string cpf)
        {
            var customerDTO = new CustomerDTO
            {
                Name = "Fulano",
                Email = "email@email.com",
                Cpf = cpf
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
            Assert.False(res.Success);

            Assert.AreEqual(res.ErrorCode, ErrorCodes.CUSTOMER_CPF_REQUIRED);
            Assert.AreEqual(res.Message, "CPF is a required information");
        }

    }
}
