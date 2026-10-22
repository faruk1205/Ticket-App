using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketingApp.Application.Tickets.GetTicket;
using TicketingApp.Application.Tickets.ReserveTicket;

namespace TicketingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ISender _sender; // MediatR'ın sadece istek gönderen arayüzü

    public TicketsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
         {
             var query = new GetTicketByIdQuery(id);
             var result = await _sender.Send(query, cancellationToken);
     
             if (result.IsFailure)
             {
                 return NotFound(new { Error = result.Error.Code, Message = result.Error.Message });
             }
     
             return Ok(result.Value);
         }

    [HttpPost("{id:guid}/reserve")]
    public async Task<IActionResult> Reserve(Guid id, CancellationToken cancellationToken)
    {
        var command = new ReserveTicketCommand(id);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new { Error = result.Error.Code, Message = result.Error.Message });
        }

        return Ok();
    }
}