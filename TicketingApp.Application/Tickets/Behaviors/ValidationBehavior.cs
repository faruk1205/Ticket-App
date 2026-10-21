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

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var firstFailure = failures.First();
            var error = new Error("Validation.Error", firstFailure.ErrorMessage);

            // Eğer TResponse Result ise
            if (typeof(TResponse) == typeof(Result))
            {
                return (TResponse)(object)Result.Failure(error);
            }

            // Eğer TResponse Result<TValue> ise
            if (typeof(TResponse).IsGenericType &&
                typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
            {
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
