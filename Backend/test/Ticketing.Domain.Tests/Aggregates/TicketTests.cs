using FluentAssertions;
using Ticketing.Domain.Enums;
using Ticketing.TestCommon.Builders;
using Ticketing.TestCommon.Fixtures;

namespace Ticketing.Domain.Tests.Aggregates;

public class TicketTests
{
  private readonly DomainFixture _fixture = new();

  [Fact]
  public void Create_Ticket_Assigns_Open_Status_And_Data_Correctly()
  {
    var user = new UserBuilder().Build();

    var ticket = new TicketBuilder()
        .WithSubject("Subject")
        .WithDescription("Description")
        .WithUser(user)
        .Build();

    ticket.Status.Should().Be(TicketStatus.Open);
    ticket.Subject.Should().Be("Subject");
    ticket.Description.Should().Be("Description");
    ticket.User.Should().Be(user);
    ticket.Replies.Should().BeEmpty();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void Cannot_Create_Ticket_With_Empty_Subject(string subject)
  {
    var user = new UserBuilder().Build();

    Action act = () => new TicketBuilder().WithSubject(subject!).WithUser(user).Build();

    act.Should().Throw<ArgumentException>();
  }

  [Theory]
  [InlineData(null)]
  [InlineData("")]
  public void Cannot_Create_Ticket_With_Empty_Description(string description)
  {
    var user = new UserBuilder().Build();

    Action act = () => new TicketBuilder().WithDescription(description!).WithUser(user).Build();

    act.Should().Throw<ArgumentException>();
  }

  [Fact]
  public void Can_Change_Status_From_Open_To_InResolution()
  {
    var ticket = _fixture.CreateTicketWithReplies(repliesCount: 3);
    
    ticket.Status.Should().Be(TicketStatus.InResolution);
  }

  [Fact]
  public void Can_Change_Status_From_InResolution_To_Resolved()
  {
    var ticket = new TicketBuilder().Build();

    ticket.MarkAsResolved();

    ticket.Status.Should().Be(TicketStatus.Resolved);
  }

  [Fact]
  public void AddReply_Adds_Reply_To_Ticket()
  {
    var agent = new UserBuilder().WithUserType(UserType.Agent).Build();
    var ticket = new TicketBuilder().Build();
    var reply = new TicketReplyBuilder().WithUser(agent).WithTicket(ticket).WithText("Reply text").Build();

    ticket.AddReply(reply);

    ticket.Replies.Should().Contain(reply);
  }

  [Fact]
  public void AddReply_Throws_If_Null_Reply()
  {
    var ticket = new TicketBuilder().Build();

    Action act = () => ticket.AddReply(null!);

    act.Should().Throw<ArgumentNullException>();
  }

  [Fact]
  public void Cannot_Add_Reply_To_Resolved_Ticket()
  {
    var agent = new UserBuilder().WithUserType(UserType.Agent).Build();
    var ticket = new TicketBuilder().Build();
    var reply = new TicketReplyBuilder().WithUser(agent).WithTicket(ticket).Build();

    ticket.MarkAsResolved();

    Action act = () => ticket.AddReply(reply);

    act.Should().Throw<InvalidOperationException>();
  }

  [Fact]
  public void Ticket_Associates_Creator_Correctly()
  {
    var user = new UserBuilder().Build();

    var ticket = new TicketBuilder().WithUser(user).Build();

    ticket.User.Should().Be(user);
    ticket.UserId.Should().Be(user.Id);
  }

  [Fact]
  public void Ticket_With_Replies_Has_Expected_Number_Of_Replies()
  {
    var ticket = _fixture.CreateTicketWithReplies(repliesCount: 3);

    ticket.Replies.Should().HaveCount(3);
  }
}
