using Application.Category;
using Application.Category.Command;
using Application.Category.Dto;
using Application.Category.Ports;
using Application.Product.Commands;
using Application.Product.Dto;
using Application.Product.Ports;
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
    }
}
