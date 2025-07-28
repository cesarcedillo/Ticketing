using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Ticketing.Ticket.Domain.Aggregates;
using Ticketing.Ticket.Domain.Entities;
using Ticketing.Ticket.Domain.Enums;
using Ticketing.Ticket.Infrastructure.Data;
using Ticketing.Ticket.Infrastructure.Data.Repositories;
using TicketType = Ticketing.Ticket.Domain.Aggregates.Ticket;

namespace Ticketing.Ticket.Infrastructure.Tests;
public class TicketRepositoryTests
{
  private readonly TicketDbContext _context;

  public TicketRepositoryTests()
  {
    var contextOptions = new DbContextOptionsBuilder<TicketDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;
    _context = new TicketDbContext(contextOptions);

    _context.Tickets.RemoveRange(_context.Tickets);
    _context.SaveChanges();

    var userId1 = Guid.NewGuid();
    var userId2 = Guid.NewGuid();

    var ticket1 = new TicketType("Cannot log in", "I always get an error when trying to log in.", userId1);
    var ticket2 = new TicketType("Question about billing", "How can I get my invoice?", userId1);

    var reply1 = new TicketReply("Can you send us a screenshot of the error?", userId2, ticket1);
    ticket1.AddReply(reply1);

    _context.Tickets.AddRange(ticket1, ticket2);
    _context.SaveChanges();
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_Ticket_With_Replies()
  {
    var repo = new TicketRepository(_context);
    var ticket = _context.Tickets.Include(t => t.Replies).First();

    var result = await repo.GetByIdAsync(ticket.Id);

    result.Should().NotBeNull();
    result.Replies.Should().NotBeNull();
  }

  [Fact]
  public async Task GetUnresolvedTicketsAsync_Should_Return_Only_Open_Or_InResolution()
  {
    var repo = new TicketRepository(_context);

    var resolvedTicket = _context.Tickets.First();
    resolvedTicket.MarkAsResolved();
    _context.SaveChanges();

    var result = await repo.GetUnresolvedTicketsAsync();

    result.Should().OnlyContain(t => t.Status != TicketStatus.Resolved);
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_Null_If_NotFound()
  {
    var repo = new TicketRepository(_context);

    var result = await repo.GetByIdAsync(Guid.NewGuid());

    result.Should().BeNull();
  }
}
