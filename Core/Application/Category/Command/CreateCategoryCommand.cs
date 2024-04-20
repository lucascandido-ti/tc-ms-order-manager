using Application.Category.Dto;
using Application.Category.Responses;
using MediatR;

namespace Application.Category.Command
{
    public class CreateCategoryCommand: IRequest<CategoryResponse>
    {
        public CategoryDTO categoryDTO { get; set; }
    }
}
