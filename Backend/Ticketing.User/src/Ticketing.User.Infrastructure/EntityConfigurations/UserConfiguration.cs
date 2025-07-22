using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserType = Ticketing.User.Domain.Aggregates.User;

namespace Ticketing.User.Infrastructure.EntityConfigurations;
public class UserConfiguration : IEntityTypeConfiguration<UserType>
{
  public void Configure(EntityTypeBuilder<UserType> builder)
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
