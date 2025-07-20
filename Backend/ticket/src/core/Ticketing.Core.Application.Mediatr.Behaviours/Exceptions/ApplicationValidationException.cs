using FluentValidation.Results;

namespace Ticketing.Core.Application.Mediatr.Behaviours.Exceptions;
public class ApplicationValidationException : Exception
{
  public ApplicationValidationException()
      : base("One or more validation failures have occurred.")
  {
    Errors = new Dictionary<string, string[]>();
  }

  public ApplicationValidationException(IEnumerable<ValidationFailure> failures)
      : this()
  {
    Errors = failures
        .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
        .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
  }

  public ApplicationValidationException(string message) : this()
  {
    Errors.Add("Application validation", [message]);
  }

  public IDictionary<string, string[]> Errors { get; }
}