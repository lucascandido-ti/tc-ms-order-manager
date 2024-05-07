using Application.Category.Command;
using Application.Category.Dto;
using Application.Category.Ports;
using Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {

        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryManager _categoryManager;
        private readonly IMediator _mediator;

        public CategoryController(
            ICategoryManager categoryManager,
            ILogger<CategoryController> logger,
            IMediator mediator)
        {
            _categoryManager = categoryManager;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> Post(CategoryDTO category)
        {
            var command = new CreateCategoryCommand
            {
                categoryDTO = category
            };

            var res = await _mediator.Send(command);

            if (res.Success) return Created("", res.Data);

            else if (res.ErrorCode == ErrorCodes.CATEGORY_NAME_REQUIRED)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.CATEGORY_DESCRIPTION_REQUIRED)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.CATEGORY_NOT_FOUND)
            {
                return BadRequest(res);
            }
            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [HttpGet]
        [Route("{categoryId}")]
        public async Task<ActionResult<CategoryDTO>> Get(int categoryId)
        {
            var res = await _categoryManager.GetCategory(categoryId);

            if (res.Success) return Created("", res.Data);

            return NotFound(res);
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> List()
        {
            var res = await _categoryManager.GetCategories();

            return Accepted("", res);
        }
    }
}
