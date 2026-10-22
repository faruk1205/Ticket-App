using FluentValidation;
using MediatR;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request, 
        RequestHandlerDelegate<TResponse> next, 
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);  //Doğrulanacak isteği bir validation context içine koyar.
 
        var validationResults = await Task.WhenAll(    //Kayıtlı validator'ların ValidateAsync metotlarını çağırır ve sonuçlarını bekler.
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults   //Tüm doğrulama sonuçlarındaki hataları tek bir listeye toplar.
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)  //Buradaki fikir şu: Geçersiz bir isteğin iş mantığını çalıştırmasını önlemek
        {
            var firstFailure = failures.First();
            var error = new Error("Validation.Error", firstFailure.ErrorMessage);

            // Eğer TResponse Result ise
            if (typeof(TResponse) == typeof(Result))  //Bu, cevap türünün tam olarak Result olup olmadığını kontrol eder.
            {
                return (TResponse)(object)Result.Failure(error);
            }

            // Eğer TResponse Result<TValue> ise
            if (typeof(TResponse).IsGenericType &&   //Bu da cevap türünün Result<TValue> biçiminde olup olmadığını kontrol eder. örneğin:TResponse = Result ise ilk kontrol eşleşir. TResponse = Result<TicketResponse> ise ikinci kontrol eşleşir.
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            { 
                //İkinci durumda kod, döndürülecek Result<TValue> türünü önceden bilmediği için Reflection kullanır:
                var valueType = typeof(TResponse).GenericTypeArguments[0];
                var failureMethod = typeof(Result)
                    .GetMethods()
                    .First(m => m.Name == nameof(Result.Failure) && m.IsGenericMethod)
                    .MakeGenericMethod(valueType);

                return (TResponse)failureMethod.Invoke(null, new object[] { error })!;
            }

            throw new ValidationException(failures);
        }

        return await next();
    }
}
//Result pattenr kullanıldığı için bu kadar karmaşık. Reflection falan kullanılmış. Exception kullanılsaydı daha basit tutulabilirdi. Ya da daha basit bir Result yapısı kullanılsaydı da daha basit tutulabilirdi
