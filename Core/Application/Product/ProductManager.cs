using Application.Product.Dto;
using Application.Product.Ports;
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

                foreach (var categoryId in request.Data.categoryIds)
                {
                    var category = await _categoryRepository.Get(categoryId);
                    if(category != null)
                    {
                        product.Categories.Add(category);
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

        public Task<ProductResponse> GetProduct(int id)
        {
            throw new NotImplementedException();
        }
    }
}
