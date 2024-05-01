using Application.Category.Dto;
using Application.Category.Requests;
using Application.Category.Responses;

namespace Application.Category.Ports
{
    public interface ICategoryManager
    {
        Task<CategoryResponse> CreateCategory(CreateCategoryRequest request);
        Task<CategoryResponse> GetCategory(int id);
        Task<List<CategoryDTO>> GetCategories();
    }
}
