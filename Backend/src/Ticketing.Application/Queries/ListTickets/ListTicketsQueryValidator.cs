﻿using FluentValidation;

namespace Ticketing.Application.Queries.ListTickets;
public class ListTicketsQueryValidator : AbstractValidator<ListTicketsQuery>
{
  public ListTicketsQueryValidator()
  {
    RuleFor(x => x.Status)
        .Must(BeAValidStatus)
        .When(x => !string.IsNullOrWhiteSpace(x.Status))
        .WithMessage("Status must be one of: Open, InResolution, Resolved.");
  }

  private bool BeAValidStatus(string? status)
  {
    if (string.IsNullOrWhiteSpace(status))
      return true;

    return status.Equals("Open", StringComparison.OrdinalIgnoreCase)
        || status.Equals("InResolution", StringComparison.OrdinalIgnoreCase)
        || status.Equals("Resolved", StringComparison.OrdinalIgnoreCase);
  }
}
