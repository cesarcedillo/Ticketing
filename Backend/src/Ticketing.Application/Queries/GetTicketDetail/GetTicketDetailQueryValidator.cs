using FluentValidation;

namespace Ticketing.Application.Queries.GetTicketDetail;
public class GetTicketDetailQueryValidator : AbstractValidator<GetTicketDetailQuery>
{
  public GetTicketDetailQueryValidator()
  {
    RuleFor(x => x.TicketId)
        .NotEmpty().WithMessage("Ticket ID is required.");
  }
}
