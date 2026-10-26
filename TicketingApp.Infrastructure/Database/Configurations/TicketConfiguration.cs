using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingApp.Domain.Entities;
using TicketingApp.Domain.ValueObjects;

namespace TicketingApp.Infrastructure.Database.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets"); // Tablo adını Domain'de değil, burada veriyoruz!

        builder.HasKey(t => t.Id);

        // Value Object (Değer Nesnesi) olan SeatNumber'ı veritabanına nasıl kaydedeceğini söylüyoruz
        builder.Property(t => t.Seat)
               .HasConversion(
                   seat => seat.Value,                      // Veritabanına yazarken string'e çevir
                   value => SeatNumber.Create(value)        // Veritabanından okurken tekrar SeatNumber yap
               )
               .HasColumnName("SeatNumber")
               .HasMaxLength(5);

        // PostgreSQL xmin sistem kolonunu Version alanı üzerinden Concurrency Token olarak tanımlıyoruz (İyimser Eşzamanlılık)
        builder.Property(t => t.Version).IsRowVersion();
    }
}