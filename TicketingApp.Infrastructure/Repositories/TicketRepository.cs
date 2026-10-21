using Microsoft.EntityFrameworkCore;
using TicketingApp.Application.Repositories;
using TicketingApp.Domain.Entities;
using TicketingApp.Infrastructure.Database;

namespace TicketingApp.Infrastructure.Repositories;

// Application katmanındaki arayüzü, Infrastructure katmanında EF Core ile dolduruyoruz.
public class TicketRepository : ITicketRepository
{
    private readonly ApplicationDbContext _context;

    public TicketRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public void Add(Ticket ticket)
    {
        _context.Tickets.Add(ticket);
    }
}