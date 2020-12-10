using NSubstitute;
using Xunit;
using System;
using Shouldly;
using IssueTracking.Domain.Shared.Issues;
using System.Threading.Tasks;
using Volo.Abp;

namespace IssueTracking.Domain.Issues
{
  public class Issue_Tests
  {

    private readonly IIssueRepository _fakeRepo;
    private readonly IssueManager _issueManager;
    private Issue _issue;
    private readonly Guid _userId;

    public Issue_Tests()
    {
      _fakeRepo = Substitute.For<IIssueRepository>();
      _issueManager = new IssueManager(_fakeRepo);
      _userId = Guid.NewGuid();
    }

    [Fact]
    public async Task Should_Set_The_CloseDate_Whenever_Close_An_IssueAsync()
    {
      _issue = await _issueManager.CreateAsync(Guid.NewGuid(), "TestIssue", "TextDescription");

      _issue.CloseDate.ShouldBeNull();

      _issue.Close(IssueCloseReason.DueDatePassed);

      _issue.IsClosed.ShouldBeTrue();
      _issue.CloseDate.ShouldNotBeNull();
    }


    [Fact]
    public async Task Should_Allow_To_ReOpen_An_IssueAsync()
    {
      _issue = await _issueManager.CreateAsync(Guid.NewGuid(), "TestIssue", "TextDescription");
      _issue.Close(IssueCloseReason.NotRelevant);

      _issue.Reopen();

      _issue.IsClosed.ShouldBeFalse();
      _issue.CloseDate.ShouldBeNull();
    }

    [Fact]
    public async Task Should_Not_Allow_To_ReOpen_A_Locked_IssueAsync()
    {
      _issue = await _issueManager.CreateAsync(Guid.NewGuid(), "TestIssue", "TextDescription");
      _issue.Close(IssueCloseReason.NotRelevant);

      _issue.Lock();

      Assert.Throws<IssueStateException>(() =>
      {
        _issue.Reopen();
      });
    }

    [Fact]
    public async Task Should_Assign_An_Issue_To_A_User()
    {
      _fakeRepo.GetIssueCountOfUserAsync(_userId).Returns(31);
      _issue = await _issueManager.CreateAsync(_userId, "Issue1Title", "Issue1Text");

      _issue.AssignedUserId.ShouldBe(_userId);
      await _fakeRepo.Received(1).GetIssueCountOfUserAsync(_userId);
    }

    [Fact]
    public async Task Should_Not_Allow_To_Assign_Issues_Over_The_Limit()
    {
      _fakeRepo.GetIssueCountOfUserAsync(_userId).Returns(IssueManager.MaxAllowedOpenIssuesForAUser);
      _issue = await _issueManager.CreateAsync(Guid.NewGuid(), "Issue1Title", "Issue1Text");

      var exception = await Assert.ThrowsAsync<BusinessException>(async () =>
      {
        await _issueManager.AssignToUserAsync(_issue, _userId);
      });

      exception.Code.ShouldBe("IM:0032");
      exception.Message.ShouldBe("You cannot assign more than 3 issues to a user!");

      _issue.AssignedUserId.ShouldBeNull();
      await _fakeRepo.Received(1).GetIssueCountOfUserAsync(_userId);
    }


  }
}