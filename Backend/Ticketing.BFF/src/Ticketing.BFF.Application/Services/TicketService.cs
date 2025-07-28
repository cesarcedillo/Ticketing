using AutoMapper;
using System.Net.Sockets;
using Ticket.Cliente.NswagAutoGen.HttpClientFactoryImplementation;
using Ticketing.BFF.Application.Dto.Responses;
using Ticketing.BFF.Application.Services.Interfaces;
using User.Cliente.NswagAutoGen.HttpClientFactoryImplementation;

namespace Ticketing.BFF.Application.Services;
public class TicketService : ITicketService
{
  private readonly ITicketClient _ticketClient;
  private readonly IUserClient _userClient;
  private readonly IMapper _mapper;

  public TicketService(ITicketClient ticketClient, IUserClient userClient, IMapper mapper)
  {
    _ticketClient = ticketClient;
    _userClient = userClient;
    _mapper = mapper;
  }

  public async Task AddReplyAsync(Guid ticketId, string text, Guid userId, CancellationToken cancellationToken)
  {
    var ticketReply = new AddTicketReplyRequest
    {
      Text = text,
      UserId = userId
    };
    await _ticketClient.AddTicketReplyAsync(ticketId, ticketReply, cancellationToken);
  }

  public async Task<Guid> CreateTicketAsync(string subject, string description, Guid userId, CancellationToken cancellationToken)
  {
    var createTicket = new CreateTicketRequest()
    {
      Subject = subject,
      Description = description,
      UserId = userId
    };

    return await _ticketClient.CreateTicketAsync(createTicket, cancellationToken);
  }

  public async Task<TicketDetailResponseBff> GetTicketDetailAsync(Guid ticketId, CancellationToken cancellationToken)
  {
    var ticket = await _ticketClient.GetTicketDetailAsync(ticketId, cancellationToken);

    var ticketDetail = _mapper.Map<TicketDetailResponseBff>(ticket);

    if (ticketDetail.Replies.Count > 0)
    {
      var userIds = new HashSet<Guid> { ticket.UserId!.Value };
      userIds.UnionWith(ticket.Replies!.Select(r => r.UserId!.Value));

      var users = await _userClient.GetUsersByIdsAsync(userIds, cancellationToken);
      var usersDict = users.ToDictionary(u => u.Id!.Value);

      ticketDetail.User = _mapper.Map<UserResponseBff>(usersDict[ticket.UserId!.Value]);
      foreach (var reply in ticketDetail.Replies)
      {
        var replyUserId = ticket.Replies!.First(r => r.Id == reply.Id).UserId!.Value;
        reply.User = _mapper.Map<UserResponseBff>(usersDict[replyUserId]);
      }
    }

    return ticketDetail;
  }

  public async Task<IReadOnlyList<TicketResponseBff>> ListTicketsAsync(string? status, Guid? userId, CancellationToken cancellationToken)
  {
    var tickets = await _ticketClient.ListTicketsAsync(status, userId, cancellationToken);

    var ticketsBff = _mapper.Map<IReadOnlyList<TicketResponseBff>>(tickets);

    if (ticketsBff.Count > 0)
    {
      var userIds = new HashSet<Guid>(tickets.Select(t => t.UserId!.Value));

      var users = await _userClient.GetUsersByIdsAsync(userIds, cancellationToken);
      var usersDict = users.ToDictionary(u => u.Id!.Value);

      foreach (var ticketBff in ticketsBff)
      {
        var _userId = tickets.First(t => t.Id == ticketBff.Id).UserId!.Value;
        ticketBff.User = _mapper.Map<UserResponseBff>(usersDict[_userId]);
      }
    }

    return ticketsBff;
  }

  public async Task MarkTicketAsResolvedAsync(Guid ticketId, CancellationToken cancellationToken)
  {
    await _ticketClient.MarkAsResolvedAsync(ticketId, cancellationToken);
  }
}
