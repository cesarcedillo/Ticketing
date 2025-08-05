using FluentAssertions;
using Ticketing.Notifications.Domain.Aggregates;
using Ticketing.Notifications.Domain.Enums;

namespace Ticketing.Notifications.Domain.Tests.Aggregates;
public class NotificationTests
{
  [Fact]
  public void Should_Create_Email_Notification()
  {
    // Arrange
    var title = "Nueva respuesta";
    var message = "Tienes una nueva respuesta en tu ticket.";
    var recipient = "usuario@email.com";
    var type = NotificationType.Email;

    // Act
    var notification = new Notification(title, message, recipient, type);

    // Assert
    notification.Id.Should().NotBeEmpty();
    notification.Title.Should().Be(title);
    notification.Message.Should().Be(message);
    notification.Recipient.Should().Be(recipient);
    notification.Type.Should().Be(NotificationType.Email);
    notification.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));
    notification.IsRead.Should().BeFalse();
  }

  [Fact]
  public void Should_Create_Front_Notification()
  {
    // Arrange
    var notification = new Notification(
        "Recordatorio",
        "Tienes un ticket pendiente de cerrar.",
        "user-123",
        NotificationType.Front);

    // Act & Assert
    notification.Type.Should().Be(NotificationType.Front);
    notification.IsRead.Should().BeFalse();
  }

  [Fact]
  public void Should_Mark_Front_Notification_As_Read()
  {
    // Arrange
    var notification = new Notification(
        "Cambio de estado",
        "Tu ticket ha sido cerrado.",
        "user-123",
        NotificationType.Front);

    // Act
    notification.MarkAsRead();

    // Assert
    notification.IsRead.Should().BeTrue();
  }

  [Fact]
  public void Should_Not_Mark_Email_Notification_As_Read()
  {
    // Arrange
    var notification = new Notification(
        "Confirmación",
        "Se ha enviado un correo.",
        "user@email.com",
        NotificationType.Email);

    // Act
    notification.MarkAsRead();

    // Assert
    notification.IsRead.Should().BeFalse();
  }
}
