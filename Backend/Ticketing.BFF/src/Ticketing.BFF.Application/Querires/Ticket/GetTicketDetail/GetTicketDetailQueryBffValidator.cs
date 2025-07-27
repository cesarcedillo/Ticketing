using FluentValidation;

namespace Ticketing.BFF.Application.Querires.Ticket.GetTicketDetail;
public class GetTicketDetailQueryValidator : AbstractValidator<GetTicketDetailQueryBff>
{
  public GetTicketDetailQueryValidator()
  {
    RuleFor(x => x.TicketId)
        .NotEmpty().WithMessage("Ticket ID is required.");
  }
}