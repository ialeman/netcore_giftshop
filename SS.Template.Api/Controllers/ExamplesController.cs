using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SS.Template.Application.Commands;
using SS.Template.Application.Examples.Commands;
using SS.Template.Application.Examples.Commands.Add;
using SS.Template.Application.Examples.Queries;
using SS.Template.Application.Examples.Queries.Get;
using SS.Template.Application.Examples.Queries.GetPage;
using SS.Template.Application.Queries;

namespace SS.Template.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExamplesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExamplesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<ExampleModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPage([FromQuery] GetExamplePageQuery query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ExampleModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _mediator.Send(new GetExampleQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Save(AddExampleModel command)
        {
            await _mediator.Send(new AddExamplesCommand(new List<AddExampleModel> { command }));
            return Ok();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(Guid id, AddExampleModel command)
        {
            await _mediator.Send(Edit.For(id, command));
            return Ok();
        }

        //[HttpDelete("{id:guid}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> Delete(Guid id)
#pragma warning disable S125 // Sections of code should not be commented out
        //{
        //    await _mediator.Send(new DeleteExampleCommand(id));
        //    return Ok();
        //}
    }

#pragma warning restore S125 // Sections of code should not be commented out
}
