using System;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Domain.Entities;

public sealed class Event : Entity
{
    public string Name { get; private set; }
    public string Location { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public bool IsActive { get; private set; }
    public uint Version { get; private set; } // Eşzamanlılık kontrolü (Concurrency Token)

    // EF Core için private parametresiz yapıcı metot
    private Event() : base(Guid.Empty)
    {
        Name = string.Empty;
        Location = string.Empty;
    }

    private Event(Guid id, string name, string location, DateTime startDate, DateTime endDate) 
        : base(id)
    {
        Name = name;
        Location = location;
        StartDate = startDate;
        EndDate = endDate;
        IsActive = true;
    }

    // Domain Factory Metodu: İş kurallarını kontrol ederek nesne oluşturur
    public static Result<Event> Create(
        Guid id, 
        string name, 
        string location, 
        DateTime startDate, 
        DateTime endDate)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<Event>(new Error("Event.EmptyName", "Etkinlik adı boş olamaz."));
        }

        if (string.IsNullOrWhiteSpace(location))
        {
            return Result.Failure<Event>(new Error("Event.EmptyLocation", "Etkinlik mekanı/lokasyonu boş olamaz."));
        }

        if (endDate < startDate)
        {
            return Result.Failure<Event>(new Error("Event.InvalidDates", "Bitiş tarihi başlangıç tarihinden önce olamaz."));
        }

        return Result.Success(new Event(id, name, location, startDate, endDate));
    }

    // İş Kuralı: Etkinliği iptal et
    public Result Cancel()
    {
        if (!IsActive)
        {
            return Result.Failure(new Error("Event.AlreadyCancelled", "Etkinlik zaten iptal edilmiş."));
        }

        IsActive = false;
        return Result.Success();
    }
}
