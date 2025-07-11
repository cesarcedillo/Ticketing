namespace Ticketing.Application.Services.Interfaces;
public interface ITicketService
{
  Task<Guid> CreateTicketAsync(string subject, string description, Guid userId, CancellationToken cancellationToken);
}
