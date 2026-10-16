using System;
using TicketingApp.Domain.Primitives;

namespace TicketingApp.Domain.Entities;

public sealed class Ticket : Entity
{
    // DİKKAT: set işlemleri private! Dışarıdan kimse kafasına göre değiştiremez.
    public Guid EventId { get; private set; }
    public string SeatNumber { get; private set; }
    public bool IsReserved { get; private set; }

    // Nesnenin sadece bizim istediğimiz şartlarda yaratılmasını zorluyoruz.
    public Ticket(Guid id, Guid eventId, string seatNumber) : base(id)
    {
        EventId = eventId;
        SeatNumber = seatNumber;
        IsReserved = false; // Yeni bilet baştan boş gelir.
    }

    // DAVRANIŞ (Behavior) - İş kuralı tam olarak burası!
    // Eskiden: public void Reserve()
    // Şimdi: Artık geriye "Result" dönüyoruz.
    public Result Reserve()
    {
        if (IsReserved)
        {
            // Eskiden throw new Exception(...) diyorduk.
            // Şimdi durumu zarifçe bildiriyoruz.
            return Result.Failure(new Error("Ticket.AlreadyReserved", "Bu bilet zaten rezerve edilmiş."));
        }

        IsReserved = true;
        
        return Result.Success();
    }
}