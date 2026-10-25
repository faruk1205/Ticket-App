using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingApp.Domain.Entities;

namespace TicketingApp.Infrastructure.Database.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable("Events");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(e => e.Location)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(e => e.StartDate)
               .IsRequired();

        builder.Property(e => e.EndDate)
               .IsRequired();

        builder.Property(e => e.IsActive)
               .IsRequired();
    }
}
