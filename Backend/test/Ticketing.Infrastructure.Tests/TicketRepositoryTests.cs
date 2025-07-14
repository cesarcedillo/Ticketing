using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Aggregates;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Enums;
using Ticketing.Infrastructure.Data;
using Ticketing.Infrastructure.Data.Repositories;

namespace Ticketing.Infrastructure.Tests;
public class TicketRepositoryTests
{
  private readonly TicketDbContext _context;

  public TicketRepositoryTests()
  {
    var contextOptions = new DbContextOptionsBuilder<TicketDbContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .Options;
    _context = new TicketDbContext(contextOptions);

    // Limpia y seed
    _context.Tickets.RemoveRange(_context.Tickets);
    _context.Users.RemoveRange(_context.Users);
    _context.SaveChanges();

    var user1 = new User("alice", new byte[] { 1, 2, 3 }, UserType.Customer);
    var user2 = new User("bob", new byte[] { 4, 5, 6 }, UserType.Agent);

    _context.Users.AddRange(user1, user2);

    var ticket1 = new Ticket("Cannot log in", "I always get an error when trying to log in.", user1);
    var ticket2 = new Ticket("Question about billing", "How can I get my invoice?", user1);

    var reply1 = new TicketReply("Can you send us a screenshot of the error?", user2, ticket1);
    ticket1.AddReply(reply1);

    _context.Tickets.AddRange(ticket1, ticket2);
    _context.SaveChanges();
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_Ticket_With_User_And_Replies()
  {
    var repo = new TicketRepository(_context);
    var ticket = _context.Tickets.Include(t => t.User).Include(t => t.Replies).First();

    var result = await repo.GetByIdAsync(ticket.Id);

    result.Should().NotBeNull();
    result!.User.Should().NotBeNull();
    result.Replies.Should().NotBeNull();
  }

  [Fact]
  public async Task GetUnresolvedTicketsAsync_Should_Return_Only_Open_Or_InResolution()
  {
    var repo = new TicketRepository(_context);

    // Resuelve un ticket para la prueba
    var resolvedTicket = _context.Tickets.First();
    resolvedTicket.MarkAsResolved();
    _context.SaveChanges();

    var result = await repo.GetUnresolvedTicketsAsync();

    result.Should().OnlyContain(t => t.Status != TicketStatus.Resolved);
  }

  [Fact]
  public void Query_Should_Return_IQueryable_With_User()
  {
    var repo = new TicketRepository(_context);

    var query = repo.Query();

    query.Should().NotBeNull();
    var ticket = query.Include(t => t.User).FirstOrDefault();
    ticket.Should().NotBeNull();
    ticket!.User.Should().NotBeNull();
  }

  [Fact]
  public async Task GetByIdAsync_Should_Return_Null_If_NotFound()
  {
    var repo = new TicketRepository(_context);

    var result = await repo.GetByIdAsync(Guid.NewGuid());

    result.Should().BeNull();
  }
}
