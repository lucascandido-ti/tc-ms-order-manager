using Application.Category.Requests;
using Application.Category.Dto;
using Application.Category;

using Domain.Category.Ports;

using Entities = Domain.Entities;
using Moq;

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

            var productEntity = CategoryDTO.MapToEntity(categoryDTO);

            return Task.FromResult(productEntity);
        }

        public Task<Entities.Category> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Entities.Category>> List()
        {
            throw new NotImplementedException();
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
    }
}
