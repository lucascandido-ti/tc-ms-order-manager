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

        public Task<List<Entities.Customer>> List()
        {
            var customerDTO = new CustomerDTO
            {
                Id = 111,
                Name = "Fulano",
                Email = "email@email.com",
                Cpf = "549.714.950-29"
            };

            var customerEntities = CustomerDTO.MapToEntity(customerDTO);

            return Task.FromResult(new List<Entities.Customer> { customerEntities });
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

        [Test]
        public async Task ShouldReturnCustomerNotFoundWhenCustomerDoesntExist()
        {
            var fakeRepo = new Mock<ICustomerRepository>();

            var fakeGuest = new Entities.Customer
            {
                Id = 333,
                Name = "Test"
            };

            fakeRepo.Setup(x => x.Get(333)).Returns(Task.FromResult<Entities.Customer?>(null));

            customerManager = new CustomerManager(fakeRepo.Object);

            var res = await customerManager.GetCustomer(333);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.CUSTOMER_NOT_FOUND);
            Assert.AreEqual(res.Message, "No customer record was found with the given Id");

        }

        [Test]
        public async Task ShouldReturnCustomerSuccess()
        {
            var fakeRepo = new Mock<ICustomerRepository>();

            var fakeCustomer = new Entities.Customer
            {
                Id = 333,
                Name = "Fulano",
                Email = "email@email.com",
                Cpf = "549.714.950-29"
            };

            fakeRepo.Setup(x => x.Get(333)).Returns(Task.FromResult((Entities.Customer?)fakeCustomer));

            customerManager = new CustomerManager(fakeRepo.Object);

            var res = await customerManager.GetCustomer(333);

            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Data.Id, fakeCustomer.Id);
            Assert.AreEqual(res.Data.Name, fakeCustomer.Name);
        }

        [Test]
        public async Task ShouldReturnListOfCustomers()
        {
            var fakeRepo = new Mock<ICustomerRepository>();

            var fakeCustomers = new List<Entities.Customer>
            {
                new Entities.Customer {
                    Id = 333,
                    Name = "Fulano",
                    Email = "email@email.com",
                    Cpf = "549.714.950-29"
                }
            };

            fakeRepo.Setup(x => x.List()).Returns(Task.FromResult<List<Entities.Customer>>(fakeCustomers));

            customerManager = new CustomerManager(fakeRepo.Object);

            var res = await customerManager.GetCustomers();

            Assert.IsNotNull(res);
            Assert.AreEqual(fakeCustomers.Count, res.Count());

            for (int i = 0; i < fakeCustomers.Count; i++)
            {
                Assert.AreEqual(fakeCustomers[i].Id, res.ElementAt(i).Id);
                Assert.AreEqual(fakeCustomers[i].Name, res.ElementAt(i).Name);
                Assert.AreEqual(fakeCustomers[i].Email, res.ElementAt(i).Email);
                Assert.AreEqual(fakeCustomers[i].Cpf, res.ElementAt(i).Cpf);
            }

        }

    }
}
