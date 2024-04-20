using Application.Category.Ports;
using Application.Category.Requests;
using Application.Category.Responses;
using Application.Customer.Requests;
using MediatR;

namespace Application.Category.Command
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
    {
        private readonly ICategoryManager _categoryManager;

        public CreateCategoryCommandHandler(ICategoryManager categoryManager)
        {
            _categoryManager = categoryManager;
        }

        public Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var req = new CreateCategoryRequest
            {
                Data = request.categoryDTO
            };

            return _categoryManager.CreateCategory(req);
        }
    }
}
