using Application.Category.Responses;
using MediatR;

namespace Application.Category.Queries
{
    public class GetCategoryQuery: IRequest<CategoryResponse>
    {
        public int Id { get; set; }
    }
}
