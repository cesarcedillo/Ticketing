using FluentValidation;

namespace Ticketing.Ticket.Application.Queries.GetTicketDetail;
public class GetTicketDetailQueryValidator : AbstractValidator<GetTicketDetailQuery>
{
  public GetTicketDetailQueryValidator()
  {
    RuleFor(x => x.TicketId)
        .NotEmpty().WithMessage("Ticket ID is required.");
  }
}
