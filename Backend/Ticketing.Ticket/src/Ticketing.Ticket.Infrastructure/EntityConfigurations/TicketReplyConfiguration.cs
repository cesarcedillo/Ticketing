using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ticketing.Ticket.Domain.Entities;

namespace Ticketing.Ticket.Infrastructure.EntityConfigurations;
public class TicketReplyConfiguration : IEntityTypeConfiguration<TicketReply>
{
  public void Configure(EntityTypeBuilder<TicketReply> builder)
  {
    builder.ToTable("TicketReplies");
    builder.HasKey(r => r.Id);

    builder.Property(r => r.Text)
        .IsRequired()
        .HasMaxLength(4000);

    builder.Property(r => r.CreatedAt)
        .IsRequired();

    builder.HasOne(r => r.Ticket)
        .WithMany(t => t.Replies)
        .HasForeignKey(r => r.TicketId)
        .OnDelete(DeleteBehavior.Cascade);
  }
}
