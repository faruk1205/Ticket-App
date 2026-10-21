using MediatR;
using Microsoft.Extensions.Logging;

namespace TicketingApp.Application.Behaviors;

// Sistemdeki her Request ve Response için bu boru hattı çalışacak
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // 1. İSTEK HANDLER'A GİTMEDEN ÖNCE:
        _logger.LogInformation("İşlem Başlıyor: {RequestName}", typeof(TRequest).Name);

        // 2. HANDLER'I ÇALIŞTIR (ReserveTicketCommandHandler burada devreye girer)
        var response = await next(); 

        // 3. İSTEK HANDLER'DAN DÖNDÜKTEN SONRA:
        _logger.LogInformation("İşlem Bitti: {RequestName}", typeof(TRequest).Name);

        return response;
    }
}