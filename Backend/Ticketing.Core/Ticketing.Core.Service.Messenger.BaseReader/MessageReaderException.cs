namespace Ticketing.Core.Service.Messenger.BaseReader;
internal class MessageReaderException(string message, Exception ex) : Exception(message, ex)
{
}