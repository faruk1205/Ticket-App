using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicketingApp.Infrastructure.Idempotency;

namespace TicketingApp.Infrastructure.Database.Configurations;

public class IdempotentRequestConfiguration : IEntityTypeConfiguration<IdempotentRequest>
{
    public void Configure(EntityTypeBuilder<IdempotentRequest> builder)
    {
        builder.ToTable("IdempotentRequests");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(r => r.Response)
               .HasColumnType("text");

        builder.Property(r => r.CreatedOnUtc)
               .IsRequired();
    }
}
