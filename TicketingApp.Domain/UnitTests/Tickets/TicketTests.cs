using TicketingApp.Domain.Entities;
using Xunit; // C# dünyasının popüler test kütüphanesi

namespace TicketingApp.Domain.UnitTests.Tickets;

public class TicketTests
{
    [Fact]
    public void Reserve_Should_ReturnSuccess_When_TicketIsNotReserved()
    {
        // 1. Arrange (Hazırlık): Tertemiz, yeni bir bilet oluştur. (Veritabanı yok, DB set yok!)
        var ticket = new Ticket(Guid.NewGuid(), Guid.NewGuid(), "A15");

        // 2. Act (Eylem): Rezerve etmeyi dene.
        var result = ticket.Reserve();

        // 3. Assert (Doğrulama): Sonucun başarılı olmasını ve biletin rezerve edilmiş olmasını bekle.
        Assert.True(result.IsSuccess);
        Assert.True(ticket.IsReserved);
    }

    [Fact]
    public void Reserve_Should_ReturnFailure_When_TicketIsAlreadyReserved()
    {
        // 1. Arrange (Hazırlık): Bilet oluştur ve BAŞTAN rezerve et.
        var ticket = new Ticket(Guid.NewGuid(), Guid.NewGuid(), "A15");
        ticket.Reserve(); // İlk rezervasyon (başarılı olur)

        // 2. Act (Eylem): İkinci kez rezerve etmeyi dene!
        var result = ticket.Reserve();

        // 3. Assert (Doğrulama): Result'ın Hata dönmesini ve hatanın "Ticket.AlreadyReserved" olmasını bekle.
        Assert.False(result.IsSuccess);
        Assert.Equal("Ticket.AlreadyReserved", result.Error.Code);
    }
}