using Application.Customer.Dto;
using Application.Product.Dto;
using Application.Product.Ports;
using Application.Product.Queries;
using Application.Product.Requests;
using Application.Product.Responses;
using Domain.Category.Ports;
using Domain.Product.Exceptions;
using Domain.Product.Ports;
using Domain.Utils;

namespace Application.Product
{
    public class ProductManager : IProductManager
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductManager(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository
            )
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<ProductResponse> CreateProduct(CreateProductRequest request)
        {
            try
            {

                var product = ProductDTO.MapToEntity(request.Data);

                foreach (var category in request.Data.Categories)
                {
                    var findCategory = await _categoryRepository.Get(category.Id);
                    if(category != null)
                    {
                        product.Categories.Add(findCategory);
                    }
                }
                
                await product.Save(_productRepository);

                request.Data.Id = product.Id;

                return new ProductResponse
                {
                    Success = true,
                    Data = request.Data,
                };
            }
            catch (ProductNameRequiredException)
            {
                return new ProductResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PRODUCT_NAME_REQUIRED,
                    Message = "Name is a required information"
                };
            }
            catch (ProductDescriptionRequiredException)
            {
                return new ProductResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PRODUCT_DESCRIPTION_REQUIRED,
                    Message = "Description is a required information"
                };
            }
            catch (ProductPriceRequiredException)
            {
                return new ProductResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PRODUCT_PRICE_REQUIRED,
                    Message = "Price is a required information"
                };
            }
            catch (ProductCategoriesRequiredException)
            {
                return new ProductResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PRODUCT_CATEGORIES_REQUIRED,
                    Message = "Category is a required information"
                };
            }
        }

        public async Task<ProductResponse> GetProduct(GetProductQuery get)
        {
            var product = await _productRepository.Get(get.Id);

            if (product == null)
            {
                return new ProductResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PRODUCT_NOT_FOUND,
                    Message = "No product record was found with the given Id"
                };
            }

            var productDto = ProductDTO.MapToDTO(product);

            return new ProductResponse
            {
                Success = true,
                Data = productDto
            };
        }

        public async Task<ProductResponse> GetProductAggregate(GetProductQuery get)
        {
            var product = await _productRepository.GetAggregate(get.Id);

            if (product == null)
            {
                return new ProductResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.PRODUCT_NOT_FOUND,
                    Message = "No product record was found with the given Id"
                };
            }

            var productDto = ProductDTO.MapToDTO(product);

            return new ProductResponse
            {
                Success = true,
                Data = productDto
            };
        }

        public async Task<List<ProductDTO>> GetProducts()
        {
            var products = await _productRepository.List();

            if (products == null || products.Count == 0)
            {
                return new List<ProductDTO> { };
            }

            var listProducts = new List<ProductDTO>();

            foreach (var product in products)
            {
                listProducts.Add(ProductDTO.MapToDTO(product));
            }

            return listProducts;
        }
    }
}
