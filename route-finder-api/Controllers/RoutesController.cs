using MediatR;
using Microsoft.AspNetCore.Mvc;
using Route.Finder.Application.Features.Commands.CreateRouteCommand;
using Route.Finder.Application.Features.Commands.DeleteRouteCommand;
using Route.Finder.Application.Features.Commands.UpdateRouteCommand;
using Route.Finder.Application.Features.Queries.GetAllRoutesQuery;
using Route.Finder.Application.Features.Queries.GetBestRouteQuery;

namespace Route.Finder.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoutesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoutesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all available routes.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var routes = await _mediator.Send(new GetAllRoutesQuery());
            return Ok(routes);
        }

        /// <summary>
        /// Get the cheapest route between origin and destination.
        /// </summary>
        [HttpGet("best")]
        public async Task<IActionResult> GetBest(
            [FromQuery] string origin,
            [FromQuery] string destination)
        {
            var query = new GetBestRouteQuery(origin, destination);
            var result = await _mediator.Send(query);

            if (result == null)
                return NotFound("No available route found between the given cities.");

            return Ok(result);
        }

        /// <summary>
        /// Create a new route.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateRouteCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
        }

        /// <summary>
        /// Update an existing route.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            Guid id,
            [FromBody] UpdateRouteCommand command)
        {
            if (id != command.Id)
                return BadRequest("Route ID mismatch.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Delete a route.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(
            Guid id)
        {
            var result = await _mediator.Send(new DeleteRouteCommand(id));
            return NoContent();
        }
    }
}
