using System;

namespace TicketingApp.Infrastructure.Idempotency;

public sealed class IdempotentRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Response { get; set; }
    public DateTime CreatedOnUtc { get; set; }
}
