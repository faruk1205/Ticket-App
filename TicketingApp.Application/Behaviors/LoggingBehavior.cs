using MediatR;
using Microsoft.Extensions.Logging;

namespace TicketingApp.Application.Behaviors;

// Sistemdeki her Request ve Response için bu boru hattı çalışacak
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>   //Burada TRequest ve TResponse generic parametrelerdir. Böylece her Command ve Query için ayrı bir LoggingBehavior yazmak zorunda kalmazsın.
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
        _logger.LogInformation("İşlem Başlıyor: {RequestName}", typeof(TRequest).Name);   //typeof(TRequest).Name -> Çalışan isteğin türünün adını alır. "İşlem Başlıyor: GetTicketByIdQuery" şeklinde logta gözükür. REFLECTİON

        // 2. HANDLER'I ÇALIŞTIR 
        var response = await next();      //Bu çağrı pipeline'ın kalanını çalıştırır. Sonraki behavior varsa ona, zincirin sonuna gelindiğinde de ilgili Handler'a ilerler. Sonuç, response değişkenine atanır.

        // 3. İSTEK HANDLER'DAN DÖNDÜKTEN SONRA:
        _logger.LogInformation("İşlem Bitti: {RequestName}", typeof(TRequest).Name);

        return response;  //Burada response değerinin Handler'dan başlayıp Controller'a kadar geri döndüğünü anlaman gerekiyor. Yani controllerdaki result'a 
    }
}