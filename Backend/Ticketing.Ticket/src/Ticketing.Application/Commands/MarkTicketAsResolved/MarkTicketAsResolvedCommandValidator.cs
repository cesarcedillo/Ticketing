using FluentValidation;

namespace Ticketing.Application.Commands.MarkTicketAsResolved;
public class MarkTicketAsResolvedCommandValidator : AbstractValidator<MarkTicketAsResolvedCommand>
{
  public MarkTicketAsResolvedCommandValidator()
  {
    RuleFor(x => x.TicketId)
        .NotEmpty().WithMessage("Ticket ID is required.");
  }
}

