namespace Ticketing.Core.Service.Messenger.Types;
public record OriginalMessageInformation
{
  public OriginalMessageInformation(string originalOrigin, string originalTopic, string failureReason)
  {
    OriginalOrigin = string.IsNullOrEmpty(originalOrigin) ? throw new ArgumentNullException(nameof(originalOrigin)) : originalOrigin;
    OriginalTopic = string.IsNullOrEmpty(originalTopic) ? throw new ArgumentNullException(nameof(originalTopic)) : originalTopic;
    FailureReason = string.IsNullOrEmpty(failureReason) ? throw new ArgumentNullException(nameof(failureReason)) : failureReason;
  }
  public string OriginalOrigin { get; }
  public string OriginalTopic { get; }
  public string FailureReason { get; }
}