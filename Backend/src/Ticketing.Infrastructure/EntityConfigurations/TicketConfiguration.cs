using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Aggregates;

namespace Ticketing.Infrastructure.EntityConfigurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
  public void Configure(EntityTypeBuilder<Ticket> builder)
  {
    builder.ToTable("Tickets");
    builder.HasKey(t => t.Id);

    builder.Property(t => t.Subject)
        .IsRequired()
        .HasMaxLength(200);

    builder.Property(t => t.Description)
        .IsRequired()
        .HasMaxLength(4000);

    builder.Property(t => t.Status)
        .IsRequired();

    builder.HasOne(t => t.User)
        .WithMany()
        .HasForeignKey(t => t.UserId)
        .OnDelete(DeleteBehavior.Restrict);
  }
}
