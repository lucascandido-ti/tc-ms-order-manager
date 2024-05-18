using Application.Order.Command;
using Application.Order.Command.CreateOrder;
using Application.Order.Dto;
using Application.Order.Queries;
using Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IMediator _mediator;

        public OrderController(ILogger<OrderController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDTO>> Post(OrderDTO order)
        {
            var command = new CreateOrderCommand
            {
                orderDTO = order
            };

            var res = await _mediator.Send(command);

            if (res.Success) return Created("", res.Data);

            else if (res.ErrorCode == ErrorCodes.ORDER_PRICE_REQUIRED)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.ORDER_PRODUCTS_REQUIRED)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.ORDER_NOT_FOUND)
            {
                return BadRequest(res);
            }
            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [HttpGet]
        [Route("{orderId}")]
        public async Task<ActionResult<OrderDTO>> Get(int orderId)
        {
            var query = new GetOrderQuery
            {
                Id = orderId
            };

            var res = await _mediator.Send(query);

            if (res.Success) return Ok(res.Data);

            else if (res.ErrorCode == ErrorCodes.ORDER_NOT_FOUND)
            {
                return BadRequest(res);
            }
            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }
        
        [HttpGet]
        public async Task<ActionResult<List<OrderDTO>>> List()
        {
            var query = new ListOrdersQuery { };
            
            var res = await _mediator.Send(query);

            if (res.Success) return Ok(res.Data);

            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }
    }
}
