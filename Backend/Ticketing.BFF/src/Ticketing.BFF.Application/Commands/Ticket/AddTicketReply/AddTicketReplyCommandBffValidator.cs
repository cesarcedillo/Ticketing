using FluentValidation;

namespace Ticketing.BFF.Application.Commands.Ticket.AddTicketReply;
public class AddTicketReplyCommandValidator : AbstractValidator<AddTicketReplyCommandBff>
{
  public AddTicketReplyCommandValidator()
  {
    RuleFor(x => x.TicketId).NotEmpty();
    RuleFor(x => x.Text).NotEmpty().MaximumLength(4000);
    RuleFor(x => x.UserId).NotEmpty();
  }
}