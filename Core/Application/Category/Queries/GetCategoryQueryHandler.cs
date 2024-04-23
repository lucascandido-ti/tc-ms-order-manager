using Application.Category.Dto;
using Application.Category.Ports;
using Application.Category.Responses;
using Domain.Category.Ports;
using MediatR;

namespace Application.Category.Queries
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryResponse>
    {

        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.Get(request.Id);

            var categoryDro = CategoryDTO.MapToDTO(category);

            return new CategoryResponse
            {
                Success = true,
                Data = categoryDro
            };
        }
    }
}
