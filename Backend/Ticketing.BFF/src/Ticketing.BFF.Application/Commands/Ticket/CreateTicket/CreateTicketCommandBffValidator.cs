using FluentValidation;

namespace Ticketing.BFF.Application.Commands.Ticket.CreateTicket;
public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommandBff>
{
  public CreateTicketCommandValidator()
  {
    RuleFor(x => x.Subject)
        .NotEmpty().WithMessage("Subject is required.")
        .MaximumLength(200).WithMessage("Subject cannot be longer than 200 characters.");

    RuleFor(x => x.Description)
        .NotEmpty().WithMessage("Description is required.")
        .MaximumLength(4000).WithMessage("Description cannot be longer than 4000 characters.");

    RuleFor(x => x.UserId)
        .NotEmpty().WithMessage("User ID is required.");
  }

}

