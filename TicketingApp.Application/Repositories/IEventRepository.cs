using TicketingApp.Domain.Entities;

namespace TicketingApp.Application.Repositories;

public interface IEventRepository
{
    Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Event @event);
}
