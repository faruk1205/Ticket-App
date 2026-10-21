using TicketingApp.Domain.Entities;

namespace TicketingApp.Application.Repositories;

public interface ITicketRepository
{
    Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Ticket ticket);
}
