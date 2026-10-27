using Microsoft.EntityFrameworkCore;
using TicketingApp.Application.Exceptions;
using TicketingApp.Application.Repositories;
using TicketingApp.Domain.Entities;
using TicketingApp.Infrastructure.Idempotency;

namespace TicketingApp.Infrastructure.Database;

// DbContext, aynı zamanda Application katmanındaki IUnitOfWork sözleşmesini yerine getiriyor!
public class ApplicationDbContext : DbContext, IUnitOfWork
{
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<IdempotentRequest> IdempotentRequests { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Entity konfigürasyonlarını (Fluent API) ayrı dosyalardan okumasını söylüyoruz
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    // IUnitOfWork sözleşmesi için SaveChangesAsync override edilir.
    // EF Core'un DbUpdateConcurrencyException'ı yakalanıp mimarimize uygun ConcurrencyException'a dönüştürülür.
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Veritabanında eşzamanlılık çakışması meydana geldi. Kayıt başka bir işlem tarafından güncellendi.", ex);
        }
    }
}