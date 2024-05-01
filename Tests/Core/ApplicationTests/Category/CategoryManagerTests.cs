using Application.Category.Requests;
using Application.Category.Dto;
using Application.Category;

using Domain.Category.Ports;

using Entities = Domain.Entities;
using Moq;
using Application.Customer;
using Domain.Customer.Ports;
using Domain.Utils;

namespace ApplicationTests.Category
{
    class FakeRepo : ICategoryRepository
    {
        public Task<Entities.Category> CreateCategory(Entities.Category category)
        {
            var categoryDTO = new CategoryDTO
            {
                Id = 111,
                Name = "Bebida",
                Description = "Drinques e Bebidas em geral"
            };

            var categoryEntity = CategoryDTO.MapToEntity(categoryDTO);

            return Task.FromResult(categoryEntity);
        }

        public Task<Entities.Category> Get(int id)
        {
            var categoryDTO = new CategoryDTO
            {
                Id = 111,
                Name = "Bebida",
                Description = "Drinques e Bebidas em geral"
            };

            var categoryEntity = CategoryDTO.MapToEntity(categoryDTO);

            return Task.FromResult(categoryEntity);
        }

        public Task<List<Entities.Category>> List()
        {
            var categoryDTO = new CategoryDTO
            {
                Id = 111,
                Name = "Bebida",
                Description = "Drinques e Bebidas em geral"
            };

            var categoryEntity = CategoryDTO.MapToEntity(categoryDTO);

            return Task.FromResult(new List<Entities.Category> { categoryEntity });
        }
    }

    public class Tests
    {
        CategoryManager categoryManager;

        [SetUp]
        public void Setup() { }

        [Test]
        public async Task ShouldBeAbleToValidateCreateCategory()
        {
            var categoryDTO = new CategoryDTO
            {
                Name = "Bebida",
                Description = "Drinques e Bebidas em geral"
            };

            var request = new CreateCategoryRequest()
            {
                Data = categoryDTO
            };

            var fakeRepo = new Mock<ICategoryRepository>();


            var expectEntity = CategoryDTO.MapToEntity(categoryDTO);
            expectEntity.Id = 111;

            fakeRepo.Setup(x => x.CreateCategory(It.IsAny<Entities.Category>()))
            .Returns(Task.FromResult(expectEntity));

            categoryManager = new CategoryManager(fakeRepo.Object);

            var res = await categoryManager.CreateCategory(request);
            Assert.IsNotNull(res);
            Assert.True(res.Success);
        }


        [Test]
        public async Task ShouldReturnCategoryNotFoundWhenCategoryDoesntExist()
        {
            var fakeRepo = new Mock<ICategoryRepository>();

            var fakeCategory = new Entities.Category
            {
                Id = 333,
                Name = "Test"
            };

            fakeRepo.Setup(x => x.Get(333)).Returns(Task.FromResult<Entities.Category?>(null));

            categoryManager = new CategoryManager(fakeRepo.Object);

            var res = await categoryManager.GetCategory(333);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.CATEGORY_NOT_FOUND);
            Assert.AreEqual(res.Message, "No category record was found with the given Id");

        }

        [Test]
        public async Task ShouldReturnCategorySuccess()
        {
            var fakeRepo = new Mock<ICategoryRepository>();

            var fakeCategory = new Entities.Category
            {
                Id = 333,
                Name = "Test",
                Description = "Test 123"
            };

            fakeRepo.Setup(x => x.Get(333)).Returns(Task.FromResult((Entities.Category?)fakeCategory));

            categoryManager = new CategoryManager(fakeRepo.Object);

            var res = await categoryManager.GetCategory(333);

            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Data.Id, fakeCategory.Id);
            Assert.AreEqual(res.Data.Name, fakeCategory.Name);
            Assert.AreEqual(res.Data.Description, fakeCategory.Description);
        }

        [Test]
        public async Task ShouldReturnListOfCategories()
        {
            var fakeRepo = new Mock<ICategoryRepository>();

            var fakeCategories = new List<Entities.Category>
            {
                new Entities.Category { Id = 1, Name = "Category1", Description = "Description1" },
                new Entities.Category { Id = 2, Name = "Category2", Description = "Description2" },
                new Entities.Category { Id = 3, Name = "Category3", Description = "Description3" }
            };

            fakeRepo.Setup(x => x.List()).Returns(Task.FromResult<List<Entities.Category>>(fakeCategories));

            var categoryManager = new CategoryManager(fakeRepo.Object);

            var res = await categoryManager.GetCategories();

            Assert.IsNotNull(res);
            Assert.AreEqual(fakeCategories.Count, res.Count());

            for (int i = 0; i < fakeCategories.Count; i++)
            {
                Assert.AreEqual(fakeCategories[i].Id, res.ElementAt(i).Id);
                Assert.AreEqual(fakeCategories[i].Name, res.ElementAt(i).Name);
                Assert.AreEqual(fakeCategories[i].Description, res.ElementAt(i).Description);
            }
        }
    }
}
