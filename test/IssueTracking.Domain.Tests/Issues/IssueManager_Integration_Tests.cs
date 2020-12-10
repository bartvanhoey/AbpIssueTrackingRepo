using System;
using System.Threading.Tasks;
using IssueTracking.Domain.Issues;
using Xunit;

namespace IssueTracking.Domain.Issues
{
  public class IssueManager_Integration_Tests : IssueTrackingDomainTestBase
  {
    private  IssueManager _issueManager;
      private readonly IIssueRepository _issueRepository;
    private Issue _issue;


    public IssueManager_Integration_Tests()
    {
      _issueRepository = GetRequiredService<IIssueRepository>();
      _issueManager = new IssueManager(_issueRepository);
    }

    [Fact]
    public async Task Should_Not_Allow_To_Assign_Issues_Over_The_LimitAsync()
    {
      // _issue = await _issueManager.CreateAsync(Guid.NewGuid(), "Issue1Title", "Issue1Text");  
      // await _issueManager.AssignToUserAsync(_issue, TestData.User1Id);

    }

  }
}