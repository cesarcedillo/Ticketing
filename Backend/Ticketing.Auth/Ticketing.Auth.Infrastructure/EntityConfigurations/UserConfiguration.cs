using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ticketing.Auth.Domain.Aggregates;

namespace Ticketing.Auth.Infrastructure.EntityConfigurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.ToTable("Users");

    builder.HasKey(u => u.Id);

    builder.Property(u => u.UserName)
        .IsRequired()
        .HasMaxLength(100);

    builder.Property(u => u.PasswordHash)
        .IsRequired();

    builder.Property(u => u.Role)
        .IsRequired()
        .HasMaxLength(20);

    builder.HasIndex(u => u.UserName)
        .IsUnique();
  }
}
