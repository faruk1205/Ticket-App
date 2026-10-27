using System;
using System.Threading;
using System.Threading.Tasks;

namespace TicketingApp.Application.Abstractions.Idempotency;

public interface IIdempotencyService
{
    Task<bool> ExistsAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task<string?> GetResponseAsync(Guid requestId, CancellationToken cancellationToken = default);
    Task CreateRequestAsync(Guid requestId, string name, string? response, CancellationToken cancellationToken = default);
}
