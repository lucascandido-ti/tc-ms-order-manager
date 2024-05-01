using Application.Category.Dto;
using Application.Customer;
using Application.Customer.Commands;
using Application.Customer.Dto;
using Application.Customer.Ports;
using Domain.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerManager _customerManager;
        private readonly IMediator _mediator;

        public CustomerController(
            ICustomerManager customerManager,
            ILogger<CustomerController> logger,
            IMediator mediator)
        {
            _customerManager = customerManager;
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDTO>> Post(CustomerDTO customer)
        {
            var command = new CreateCustomerCommand
            {
                customerDTO = customer
            };

            var res = await _mediator.Send(command);

            if (res.Success) return Created("", res.Data);

            else if (res.ErrorCode == ErrorCodes.CUSTOMER_NAME_REQUIRED)
            {
                return BadRequest(res);
            }
            else if (res.ErrorCode == ErrorCodes.CUSTOMER_EMAIL_REQUIRED)
            {
                return BadRequest(res);
            }else if (res.ErrorCode == ErrorCodes.CUSTOMER_CPF_REQUIRED)
            {
                return BadRequest(res);
            }
            _logger.LogError("Response with unknown ErrorCode Returned", res);
            return BadRequest(500);
        }

        [HttpGet]
        public async Task<ActionResult<CustomerDTO>> Get(int customerId)
        {
            var res = await _customerManager.GetCustomer(customerId);

            if (res.Success) return Created("", res.Data);

            return NotFound(res);
        }

        [HttpGet]
        [Route("list")]
        public async Task<ActionResult<List<CustomerDTO>>> List()
        {
            var res = await _customerManager.GetCustomers();

            return Accepted("", res);
        }
    }
}
