using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ticketing.Domain.Entities;

namespace Ticketing.Infrastructure.EntityConfigurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.ToTable("Users");
    builder.HasKey(u => u.Id);

    builder.Property(u => u.UserName)
        .IsRequired()
        .HasMaxLength(100);

    builder.Property(u => u.Avatar)
        .IsRequired()
        .HasMaxLength(4000);

    builder.Property(u => u.UserType)
        .IsRequired();

    builder.HasIndex(u => u.UserName).IsUnique();
  }
}
