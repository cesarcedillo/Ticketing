using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.Notifications.Domain.Aggregates;

namespace Ticketing.Notifications.Infrastructure.EntityConfigurations;
public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
  public void Configure(EntityTypeBuilder<Notification> builder)
  {
    builder.ToTable("Notifications");

    builder.HasKey(n => n.Id);

    builder.Property(n => n.Title)
           .IsRequired()
           .HasMaxLength(200);

    builder.Property(n => n.Message)
           .IsRequired()
           .HasMaxLength(1000);

    builder.Property(n => n.Recipient)
           .IsRequired()
           .HasMaxLength(200);

    builder.Property(n => n.Type)
           .HasConversion<int>()
           .IsRequired();

    builder.Property(n => n.IsRead)
           .IsRequired();

    builder.Property(n => n.CreatedAt)
           .IsRequired();
  }
}

