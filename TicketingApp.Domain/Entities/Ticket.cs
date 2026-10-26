using System;
using TicketingApp.Domain.Primitives;
using TicketingApp.Domain.ValueObjects;

namespace TicketingApp.Domain.Entities;

public sealed class Ticket : Entity
{
    // DİKKAT: set işlemleri private! Dışarıdan kimse kafasına göre değiştiremez.
    public Guid EventId { get; private set; }
    public SeatNumber Seat { get; private set; }
    public bool IsReserved { get; private set; }
    public uint Version { get; private set; } // Eşzamanlılık kontrolü (Concurrency Token)

    // EF Core için parametresiz kurucu
    private Ticket() : base(Guid.Empty)
    {
        Seat = null!;
    }

    public Ticket(Guid id, Guid eventId, SeatNumber seat) : base(id)
    {
        EventId = eventId;
        Seat = seat;
        IsReserved = false; // Yeni bilet baştan boş gelir.
    }

    // Geriye dönük uyumluluk ve kolay kullanım için string alan aşırı yükleme (overload)
    public Ticket(Guid id, Guid eventId, string seatNumber) 
        : this(id, eventId, SeatNumber.Create(seatNumber))
    {
    }

    // DAVRANIŞ (Behavior) - İş kuralı tam olarak burası!
    public Result Reserve()
    {
        if (IsReserved)
        {
            return Result.Failure(new Error("Ticket.AlreadyReserved", "Bu bilet zaten rezerve edilmiş."));
        }

        IsReserved = true;
        
        return Result.Success();
    }
}