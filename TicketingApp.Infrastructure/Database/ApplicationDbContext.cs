using Microsoft.EntityFrameworkCore;
using TicketingApp.Application.Repositories;
using TicketingApp.Domain.Entities;

namespace TicketingApp.Infrastructure.Database;

// DbContext, aynı zamanda Application katmanındaki IUnitOfWork sözleşmesini yerine getiriyor!
public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public DbSet<Ticket> Tickets { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Entity konfigürasyonlarını (Fluent API) ayrı dosyalardan okumasını söylüyoruz
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    // IUnitOfWork içindeki SaveChangesAsync zaten DbContext'in içinde var olduğu için 
    // ekstra bir şey yazmamıza gerek kalmıyor, direkt arayüzü karşılıyor.
}