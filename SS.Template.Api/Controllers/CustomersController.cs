using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.Template.Application.Customers;
using SS.Template.Application.Queries;

namespace SS.Template.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersService _customersService;

        public CustomersController(ICustomersService customersService)
        {
            _customersService = customersService;
        }

        // GET: api/Customers
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<CustomerModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get([FromQuery] PaginatedQuery query)
        {
            var page = await _customersService.GetPage(query);
            return Ok(page);
        }

        // GET: api/Customers/5
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CustomerModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var customer = await _customersService.Get(id);
            return Ok(customer);
        }

        // POST: api/Customers
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save([FromBody] CustomerModel customer)
        {
            await _customersService.Create(customer);
            return Ok();
        }

        // PUT: api/Customers/5
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CustomerModel customer)
        {
            await _customersService.Update(id, customer);
            return Ok();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _customersService.Delete(id);
            return Ok();
        }
    }
}
