using Microsoft.EntityFrameworkCore;
using TicketingApp.Application.Repositories;
using TicketingApp.Domain.Entities;
using TicketingApp.Infrastructure.Database;

namespace TicketingApp.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext _context;

    public EventRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Events.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Add(Event @event)
    {
        _context.Events.Add(@event);
    }
}
