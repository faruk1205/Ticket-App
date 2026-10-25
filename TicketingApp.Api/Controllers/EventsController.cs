using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketingApp.Application.Events.CreateEvent;
using TicketingApp.Application.Events.GetEvent;
using TicketingApp.Application.Events.GetEvents;

namespace TicketingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ISender _sender;

    public EventsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetEventsQuery();
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { Error = result.Error.Code, Message = result.Error.Message });
        }

        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetEventByIdQuery(id);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(new { Error = result.Error.Code, Message = result.Error.Message });
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEventRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateEventCommand(
            request.Name, 
            request.Location, 
            request.StartDate, 
            request.EndDate);

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { Error = result.Error.Code, Message = result.Error.Message });
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { Id = result.Value });
    }
}

public record CreateEventRequest(string Name, string Location, DateTime StartDate, DateTime EndDate);
