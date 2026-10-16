using Microsoft.AspNetCore.Mvc;
using TicketingApp.Application.Tickets.ReserveTicket;

namespace TicketingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ReserveTicketHandler _handler;

    public TicketsController(ReserveTicketHandler handler)
    {
        _handler = handler;
    }

    [HttpPost("{id:guid}/reserve")]
    public async Task<IActionResult> Reserve(Guid id, CancellationToken cancellationToken)
    {
        // 1. İsteği Handler'a fırlat. Bütün işi o yapsın.
        var result = await _handler.HandleAsync(id, cancellationToken);

        // 2. İşlem başarısızsa HTTP 400 (Bad Request) dön. 
        // Domain'den gelen zarif Error nesnesini doğrudan API cevabı yapıyoruz.
        if (result.IsFailure)
        {
            // İleride bunu RFC standartlarına uygun "ProblemDetails" formatına çevireceğiz
            return BadRequest(new { Error = result.Error.Code, Message = result.Error.Message });
        }

        // 3. Başarılıysa HTTP 200 (OK) dön.
        return Ok();
    }
}