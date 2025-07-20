using FluentValidation;

namespace Ticketing.Application.Commands.AddTicketReply;
public class AddTicketReplyCommandValidator : AbstractValidator<AddTicketReplyCommand>
{
  public AddTicketReplyCommandValidator()
  {
    RuleFor(x => x.TicketId).NotEmpty();
    RuleFor(x => x.Text).NotEmpty().MaximumLength(4000);
    RuleFor(x => x.UserId).NotEmpty();
  }
}
