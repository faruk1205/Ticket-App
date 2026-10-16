namespace TicketingApp.Application.Repositories;

// Veritabanındaki değişiklikleri (transaction) tek seferde kaydetmek için sözleşme.
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}