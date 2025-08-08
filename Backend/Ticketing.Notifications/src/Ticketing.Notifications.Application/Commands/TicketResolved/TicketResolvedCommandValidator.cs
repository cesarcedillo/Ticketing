using FluentValidation;

namespace Ticketing.Notifications.Application.Commands.TicketResolved;
public class TicketResolvedCommandValidator : AbstractValidator<TicketResolvedCommand>
{
  public TicketResolvedCommandValidator()
  {
    RuleFor(x => x.TicketId).NotEmpty();
    RuleFor(x => x.UserId).NotEmpty();
  }
}
