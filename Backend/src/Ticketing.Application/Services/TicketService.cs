using Ticketing.Application.Services.Interfaces;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Interfaces.Repositories;

namespace Ticketing.Application.Services;

public class TicketService : ITicketService
{
  private readonly ITicketRepository _ticketRepository;
  private readonly IUserRepository _userRepository;

  public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository)
  {
    _ticketRepository = ticketRepository;
    _userRepository = userRepository;
  }

  public async Task<Guid> CreateTicketAsync(string subject, string description, Guid userId, CancellationToken cancellationToken)
  {
    var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

    if (user == null)
      throw new InvalidOperationException("User not found.");

    var ticket = new Ticket(subject, description, user);

    _ticketRepository.Add(ticket);

    await _ticketRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

    return ticket.Id;
  }
}

