using Application.Customer.Dto;
using Application.Product.Commands;
using Application.Product.Dto;
using Application.Product.Ports;
using Application.Product.Queries;
using Domain.Entities;
using Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : Controller
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IProductManager _productManager;
        private readonly IMediator _mediator;

        public ProductController(
            IProductManager productManager,
            ILogger<ProductController> logger,
            IMediator mediator)
        {
            _productManager = productManager;
            _logger = logger;
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<ActionResult<ProductDTO>> Post(ProductDTO product)
        {
            var command = new CreateProductCommand
            {
                productDTO = product
            };

            var res = await _mediator.Send(command);

            if (res.Success) return Created("", res.Data);

            else if (res.ErrorCode == ErrorCodes.PRODUCT_NAME_REQUIRED)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.PRODUCT_DESCRIPTION_REQUIRED)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.PRODUCT_CATEGORIES_REQUIRED)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.PRODUCT_PRICE_REQUIRED)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.PRODUCT_NOT_FOUND)
            {
                return BadRequest(res);
            }
            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<ProductDTO>> Get(int productId)
        {
            var query = new GetProductQuery
            {
                Id = productId
            };

            var res = await _mediator.Send(query);

            if (res.Success) return Created("", res.Data);

            return NotFound(res);
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<List<ProductDTO>>> List()
        {
            var res = await _productManager.GetProducts();

            return Accepted("", res);
        }

        [HttpGet]
        [Route("categories")]
        public async Task<ActionResult<ProductDTO>> GetAggregate(int productId)
        {
            var query = new GetProductAggregateQuery
            {
                Id = productId
            };

            var res = await _mediator.Send(query);

            if (res.Success) return Created("", res.Data);

            return NotFound(res);
        }
    }
}
