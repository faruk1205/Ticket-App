using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketingApp.Application.Abstractions.Idempotency;
using TicketingApp.Infrastructure.Database;

namespace TicketingApp.Infrastructure.Idempotency;

public class IdempotencyService : IIdempotencyService
{
    private readonly ApplicationDbContext _context;

    public IdempotencyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistsAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        return await _context.IdempotentRequests
            .AnyAsync(r => r.Id == requestId, cancellationToken);
    }

    public async Task<string?> GetResponseAsync(Guid requestId, CancellationToken cancellationToken = default)
    {
        return await _context.IdempotentRequests
            .Where(r => r.Id == requestId)
            .Select(r => r.Response)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateRequestAsync(
        Guid requestId, 
        string name, 
        string? response, 
        CancellationToken cancellationToken = default)
    {
        var idempotentRequest = new IdempotentRequest
        {
            Id = requestId,
            Name = name,
            Response = response,
            CreatedOnUtc = DateTime.UtcNow
        };

        _context.IdempotentRequests.Add(idempotentRequest);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
