using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TicketingApp.Application.Exceptions;

namespace TicketingApp.Api.Infrastructure;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // 1. Hatayı analiz et ve uygun HTTP durum kodunu belirle
        var (statusCode, title, detail, errors) = MapException(exception);

        // 2. Hatayı logla (500 sunucu hataları Error, bilinen istemci/iş kuralı hataları Warning)
        if (statusCode >= StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(exception, "Sunucu tarafında işlenmemiş bir hata meydana geldi: {Message}", exception.Message);
        }
        else
        {
            _logger.LogWarning("İstemci kaynaklı veya iş kuralı hatası yakalandı ({StatusCode}): {Message}", statusCode, exception.Message);
        }

        // 3. RFC 7807 uyumlu ProblemDetails nesnesi hazırla
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        if (errors is not null)
        {
            problemDetails.Extensions["errors"] = errors;
        }

        // 4. Yanıtı istemciye JSON olarak yaz
        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true; // Pipeline'a hatanın başarıyla ele alındığını bildirir
    }

    private static (int StatusCode, string Title, string Detail, object? Errors) MapException(Exception exception)
    {
        return exception switch
        {
            ValidationException validationException => (
                StatusCodes.Status400BadRequest,
                "Doğrulama Hatası",
                "Gönderilen veriler doğrulama kurallarından geçemedi.",
                validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )
            ),

            ConcurrencyException concurrencyException => (
                StatusCodes.Status409Conflict,
                "Eşzamanlılık Çatışması",
                concurrencyException.Message,
                null
            ),

            BadHttpRequestException badHttpRequestException => (
                StatusCodes.Status400BadRequest,
                "Geçersiz İstek",
                badHttpRequestException.Message,
                null
            ),

            _ => (
                StatusCodes.Status500InternalServerError,
                "Sunucu Hatası",
                "İşlem sırasında beklenmeyen bir hata meydana geldi.",
                null
            )
        };
    }
}
