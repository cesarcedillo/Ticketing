using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Ticketing.Domain.Aggregates;

namespace Ticketing.Infrastructure.EntityConfigurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
  [ExcludeFromCodeCoverage]
  public void Configure(EntityTypeBuilder<Ticket> builder)
  {
    builder.HasKey(p => p.TicketId);
    builder.Property(p => p.TicketId)
        .IsRequired()
        .ValueGeneratedNever();
  }
}