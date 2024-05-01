using Application.Category.Dto;
using Application.Product;
using Application.Product.Dto;
using Application.Product.Requests;
using Domain.Category.Ports;
using Domain.Product.Ports;
using Entities = Domain.Entities;
using Moq;

namespace ApplicationTests.Product
{
    class FakeRepoProduct : IProductRepository
    {
        public Task<Entities.Product> CreateProduct(Entities.Product product)
        {
            var productDTO = new ProductDTO
            {
                Id = 111,
                Name = "Milkshake",
                Description = "Sobremesa e Bebidas",
                Categories = new List<CategoryDTO> {
                    new CategoryDTO
                    {
                        Id = 111
                    }
                },
                Price = 12.99m,
                Currency = 0
            };

            var productEntity = ProductDTO.MapToEntity(productDTO);

            return Task.FromResult(productEntity);
        }

        public Task<Entities.Product> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Entities.Product> GetAggregate(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Entities.Product>> List()
        {
            throw new NotImplementedException();
        }
    }

    class FakeRepoCategory : ICategoryRepository
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
            throw new NotImplementedException();
        }
    }

    public class Tests
    {
        ProductManager productManager;

        [SetUp]
        public void Setup() { }

        [Test]
        public async Task ShouldBeAbleToValidateCreateProduct()
        {
            var productDTO = new ProductDTO
            {
                Name = "Milkshake",
                Description = "Sobremesa e Bebidas",
                Price = 12.99m,
                Currency = 0,
                Categories = new List<CategoryDTO> {
                    new CategoryDTO
                    {
                        Id = 111
                    }
                }
                    
            };

            var request = new CreateProductRequest()
            {
                Data = productDTO
            };

            var fakeRepoProduct = new Mock<IProductRepository>();
            var fakeRepoCategory = new Mock<ICategoryRepository>();


            var expectEntity = ProductDTO.MapToEntity(productDTO);
            expectEntity.Id = 111;

            fakeRepoProduct.Setup(x => x.CreateProduct(It.IsAny<Entities.Product>()))
            .Returns(Task.FromResult(expectEntity));

            productManager = new ProductManager(fakeRepoProduct.Object, fakeRepoCategory.Object);

            var res = await productManager.CreateProduct(request);
            Assert.IsNotNull(res);
            Assert.True(res.Success);
        }

    }
}
