using Application.Category.Dto;
using Application.Product;
using Application.Product.Dto;
using Application.Product.Requests;
using Domain.Category.Ports;
using Domain.Product.Ports;
using Entities = Domain.Entities;
using Moq;
using Application.Category;
using ValueObjects = Domain.Product.ValueObjects;
using Domain.Utils.Enums;
using Application.Category.Queries;
using Application.Product.Queries;
using Domain.Product.ValueObjects;
using System.Xml.Linq;
using Application.Customer;
using Domain.Customer.Ports;
using Domain.Utils;

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

        public Task<Entities.Product> GetAggregate(int id)
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

        public Task<List<Entities.Product>> List()
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

            return Task.FromResult(new List<Entities.Product> { productEntity });
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

        [Test]
        public async Task ShouldReturnProductNotFoundWhenProductDoesntExist()
        {
            var fakeProductRepo = new Mock<IProductRepository>();
            var fakeCategoryRepo = new Mock<ICategoryRepository>();

            var fakeProduct = new Entities.Product
            {
                Id = 333,
                Name = "Milkshake",
                Description = "Sobremesa e Bebidas",
                Price = new ValueObjects.Price { Currency = AcceptedCurrencies.Real, Value = 12.99m },
                Categories = new List<Entities.Category>()
                {
                    CategoryDTO.MapToEntity(new CategoryDTO
                    {
                        Id = 111,
                        Name = "Bebida",
                        Description = "Drinques e Bebidas em geral"
                    })
                }

            };

            fakeProductRepo.Setup(x => x.Get(333)).Returns(Task.FromResult<Entities.Product?>(null));

            productManager = new ProductManager(fakeProductRepo.Object, fakeCategoryRepo.Object);

            var query = new GetProductQuery
            {
                Id = 333
            };

            var res = await productManager.GetProduct(query);

            Assert.IsNotNull(res);
            Assert.False(res.Success);
            Assert.AreEqual(res.ErrorCode, ErrorCodes.PRODUCT_NOT_FOUND);
            Assert.AreEqual(res.Message, "No product record was found with the given Id");

        }

        [Test]
        public async Task ShouldReturnProductSuccess()
        {
            var fakeProductRepo = new Mock<IProductRepository>();
            var fakeCategoryRepo = new Mock<ICategoryRepository>();

            var fakeProduct = new Entities.Product
            {
                Id = 333,
                Name = "Milkshake",
                Description = "Sobremesa e Bebidas",
                Price = new ValueObjects.Price { Currency = AcceptedCurrencies.Real, Value = 12.99m },
                Categories = new List<Entities.Category>()
                {
                    CategoryDTO.MapToEntity(new CategoryDTO
                    {
                        Id = 111,
                        Name = "Bebida",
                        Description = "Drinques e Bebidas em geral"
                    })
                }

            };

            fakeProductRepo.Setup(x => x.Get(333)).Returns(Task.FromResult((Entities.Product?)fakeProduct));

            productManager = new ProductManager(fakeProductRepo.Object, fakeCategoryRepo.Object);

            var query = new GetProductQuery
            {
                Id = 333
            };

            var res = await productManager.GetProduct(query);

            Assert.IsNotNull(res);
            Assert.True(res.Success);
            Assert.AreEqual(res.Data.Id, fakeProduct.Id);
            Assert.AreEqual(res.Data.Name, fakeProduct.Name);
            Assert.AreEqual(res.Data.Description, fakeProduct.Description);
            Assert.AreEqual(res.Data.Price, fakeProduct.Price.Value);

            for (int i = 0; i < fakeProduct.Categories.Count; i++)
            {
                var fakeCategories = new List<Entities.Category>(fakeProduct.Categories);

                Assert.AreEqual(fakeCategories[i].Id, res.Data.Categories.ElementAt(i).Id);
                Assert.AreEqual(fakeCategories[i].Name, res.Data.Categories.ElementAt(i).Name);
                Assert.AreEqual(fakeCategories[i].Description, res.Data.Categories.ElementAt(i).Description);
            }
            
        }

        [Test]
        public async Task ShouldReturnListOfProducts()
        {
            var fakeProductRepo = new Mock<IProductRepository>();
            var fakeCategoryRepo = new Mock<ICategoryRepository>();


            var fakeProduct = new List<Entities.Product>();

            for(int i = 0; i < 2; i++){
                fakeProduct.Add(ProductDTO.MapToEntity(new ProductDTO
                {
                    Id = i,
                    Name = "Product"+i,
                    Description = "Description"+i,
                    Price = 12.99m,
                    Currency = 0
                }));
            }

            fakeProductRepo.Setup(x => x.List()).Returns(Task.FromResult<List<Entities.Product>>(fakeProduct));

            var productManager = new ProductManager(fakeProductRepo.Object, fakeCategoryRepo.Object);

            var res = await productManager.GetProducts();

            Assert.IsNotNull(res);
            Assert.AreEqual(fakeProduct.Count, res.Count());

            for (int i = 0; i < fakeProduct.Count; i++)
            {
                Assert.AreEqual(fakeProduct[i].Id, res.ElementAt(i).Id);
                Assert.AreEqual(fakeProduct[i].Name, res.ElementAt(i).Name);
                Assert.AreEqual(fakeProduct[i].Description, res.ElementAt(i).Description);
                Assert.AreEqual(fakeProduct[i].Price.Value, res.ElementAt(i).Price);
            }
        }

    }
}
