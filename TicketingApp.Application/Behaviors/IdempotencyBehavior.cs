using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TicketingApp.Application.Abstractions.Idempotency;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Behaviors;

public class IdempotencyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IIdempotentCommand<TResponse>
    where TResponse : Result
{
    private readonly IIdempotencyService _idempotencyService;

    public IdempotencyBehavior(IIdempotencyService idempotencyService)
    {
        _idempotencyService = idempotencyService;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        // Eğer istemci bir RequestId (Idempotency-Key) göndermediyse boru hattına normal devam et
        if (request.RequestId == Guid.Empty)
        {
            return await next();
        }

        // 1. Bu istek daha önce başarıyla işlenmiş mi?
        if (await _idempotencyService.ExistsAsync(request.RequestId, cancellationToken))
        {
            // Eğer yanıt doğrudan Result türündeyse
            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Success();
            }

            // Eğer yanıt Result<TValue> türündeyse
            if (typeof(TResponse).IsGenericType && 
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var responseJson = await _idempotencyService.GetResponseAsync(request.RequestId, cancellationToken);
                var valueType = typeof(TResponse).GenericTypeArguments[0];
                
                var deserializedValue = string.IsNullOrWhiteSpace(responseJson)
                    ? null
                    : JsonSerializer.Deserialize(responseJson, valueType);

                var successMethod = typeof(Result)
                    .GetMethods()
                    .First(m => m.Name == nameof(Result.Success) && m.IsGenericMethod)
                    .MakeGenericMethod(valueType);

                return (TResponse)successMethod.Invoke(null, new[] { deserializedValue })!;
            }

            return (TResponse)(object)Result.Success();
        }

        // 2. İstek ilk defa geliyorsa Handler'ı çalıştır
        var response = await next();

        // 3. İşlem başarılı olduysa idempotency kaydını oluştur
        if (response.IsSuccess)
        {
            string? serializedValue = null;

            if (typeof(TResponse).IsGenericType && 
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
                var valueProperty = typeof(TResponse).GetProperty("Value");
                var value = valueProperty?.GetValue(response);
                serializedValue = JsonSerializer.Serialize(value);
            }

            await _idempotencyService.CreateRequestAsync(
                request.RequestId, 
                typeof(TRequest).Name, 
                serializedValue, 
                cancellationToken);
        }

        return response;
    }
}
