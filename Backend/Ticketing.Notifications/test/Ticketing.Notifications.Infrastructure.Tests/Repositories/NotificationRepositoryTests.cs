using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Ticketing.Notifications.Domain.Aggregates;
using Ticketing.Notifications.Domain.Enums;
using Ticketing.Notifications.Infrastructure.Data;
using Ticketing.Notifications.Infrastructure.Data.Repositories;

namespace Ticketing.Notifications.Infrastructure.Tests.Repositories;
public class NotificationRepositoryTests
{
  private readonly NotificationsDbContext _context;

  public NotificationRepositoryTests()
  {
    var options = new DbContextOptionsBuilder<NotificationsDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;

    _context = new NotificationsDbContext(options);

    _context.Notifications.RemoveRange(_context.Notifications);
    _context.SaveChanges();

    var n1 = new Notification("Título 1", "Mensaje 1", "user1", NotificationType.Email);
    var n2 = new Notification("Título 2", "Mensaje 2", "user1", NotificationType.Front);
    var n3 = new Notification("Título 3", "Mensaje 3", "user2", NotificationType.Front);

    _context.Notifications.AddRange(n1, n2, n3);
    _context.SaveChanges();
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_Notification_When_Exists()
  {
    var repo = new NotificationRepository(_context);
    var notification = await _context.Notifications.FirstAsync();

    var result = await repo.GetByIdAsync(notification.Id);

    result.Should().NotBeNull();
    result!.Id.Should().Be(notification.Id);
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
  {
    var repo = new NotificationRepository(_context);

    var result = await repo.GetByIdAsync(Guid.NewGuid());

    result.Should().BeNull();
  }

  [Fact]
  public async Task GetByRecipientAsync_Should_Return_Notifications_When_Exist()
  {
    var repo = new NotificationRepository(_context);

    var result = await repo.GetByRecipientAsync("user1");

    result.Should().NotBeNull();
    result.Should().HaveCount(2);
    result.Should().OnlyContain(n => n.Recipient == "user1");
  }

  [Fact]
  public async Task GetByRecipientAsync_Should_Return_Empty_When_None_Exist()
  {
    var repo = new NotificationRepository(_context);

    var result = await repo.GetByRecipientAsync("no-user");

    result.Should().NotBeNull();
    result.Should().BeEmpty();
  }
}
