using Application.Category.Dto;
using Application.Category.Ports;
using Application.Category.Requests;
using Application.Category.Responses;
using Domain.Category.Exceptions;
using Domain.Category.Ports;
using Domain.Utils;

namespace Application.Category
{
    public class CategoryManager : ICategoryManager
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryManager(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse> CreateCategory(CreateCategoryRequest request)
        {
            try
            {
                var category = CategoryDTO.MapToEntity(request.Data);

                await category.Save(_categoryRepository);

                request.Data.Id = category.Id;

                return new CategoryResponse
                {
                    Success = true,
                    Data = request.Data,
                };
            }
            catch (CategoryNameRequiredException)
            {
                return new CategoryResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.CATEGORY_NAME_REQUIRED,
                    Message = "Name is a required information"
                };
            }
            catch (CategoryDescriptionRequiredException)
            {
                return new CategoryResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.CATEGORY_DESCRIPTION_REQUIRED,
                    Message = "Description is a required information"
                };
            }
        }

        public Task<CategoryResponse> GetCategory(int id)
        {
            throw new NotImplementedException();
        }
    }
}
