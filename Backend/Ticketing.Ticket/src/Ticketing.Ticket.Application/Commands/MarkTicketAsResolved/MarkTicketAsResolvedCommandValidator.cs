using FluentValidation;

namespace Ticketing.Ticket.Application.Commands.MarkTicketAsResolved;
public class MarkTicketAsResolvedCommandValidator : AbstractValidator<MarkTicketAsResolvedCommand>
{
  public MarkTicketAsResolvedCommandValidator()
  {
    RuleFor(x => x.TicketId)
        .NotEmpty().WithMessage("Ticket ID is required.");
  }
}

