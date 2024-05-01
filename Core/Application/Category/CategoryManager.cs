using Application.Category.Dto;
using Application.Category.Ports;
using Application.Category.Requests;
using Application.Category.Responses;
using Application.Customer.Dto;
using Application.Customer.Responses;
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

        public async Task<CategoryResponse> GetCategory(int id)
        {
            var category = await _categoryRepository.Get(id);

            if (category == null)
            {
                return new CategoryResponse
                {
                    Success = false,
                    ErrorCode = ErrorCodes.CATEGORY_NOT_FOUND,
                    Message = "No category record was found with the given Id"
                };
            }

            var categoryDto = CategoryDTO.MapToDTO(category);

            return new CategoryResponse
            {
                Success = true,
                Data = categoryDto
            };
        }

        public async Task<List<CategoryDTO>> GetCategories()
        {
            var categories = await _categoryRepository.List();

            if (categories == null || categories.Count == 0)
            {
                return new List<CategoryDTO> { };
            }

            var listCategories = new List<CategoryDTO>();

            foreach(var category in categories) {
                listCategories.Add(CategoryDTO.MapToDTO(category));
            }

            return listCategories;
        }

    }
}
