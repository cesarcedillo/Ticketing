using FluentValidation;

namespace Ticketing.BFF.Application.Commands.Ticket.MarkTicketAsResolved;
public class MarkTicketAsResolvedCommandValidator : AbstractValidator<MarkTicketAsResolvedCommandBff>
{
  public MarkTicketAsResolvedCommandValidator()
  {
    RuleFor(x => x.TicketId)
        .NotEmpty().WithMessage("Ticket ID is required.");
  }
}

