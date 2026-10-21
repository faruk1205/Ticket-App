using MediatR;
using Microsoft.AspNetCore.Mvc;
using TicketingApp.Application.Tickets.ReserveTicket;

namespace TicketingApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ISender _sender; // MediatR'ın sadece istek gönderen arayüzü

    // Artık 100 tane Handler'a ihtiyacımız yok. Sadece ISender yeterli.
    public TicketsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost("{id:guid}/reserve")]
    public async Task<IActionResult> Reserve(Guid id, CancellationToken cancellationToken)
    {
        var command = new ReserveTicketCommand(id);
        
        // Sender, doğru handler'ı otomatik bulup çalıştıracak!
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailure)
            return BadRequest(new { Error = result.Error.Code, Message = result.Error.Message });

        return Ok();
    }
}

/*using Microsoft.AspNetCore.Mvc;
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
*/