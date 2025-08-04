namespace Ticketing.Core.Service.Messenger.Readers;
internal class MessageReaderException(string message, Exception ex) : Exception(message, ex)
{
}