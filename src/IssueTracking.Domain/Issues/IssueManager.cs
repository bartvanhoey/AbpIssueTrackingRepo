using System;
using System.Threading.Tasks;
using IssueTracking.Users;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace IssueTracking.Domain.Issues
{
  public class IssueManager : DomainService
  {
    private readonly IRepository<Issue, Guid> _issueRepository;

    public IssueManager(IRepository<Issue, Guid> issueRepository)
    {
      _issueRepository = issueRepository;
    }

    public async Task AssignToAsync(Issue issue, AppUser user)
    {
      var openIssueCount = await _issueRepository.CountAsync(issue => issue.AssignedUserId == user.Id && !issue.IsClosed);

      if (openIssueCount >= 3)
      {
        throw new BusinessException("IssueTracking:ConcurrentOpenIssueLimit");
      }
      issue.AssignedUserId = user.Id;
    }

    public async Task<Issue> CreateAsync(Guid repositoryId, string title, string text = null)
    {
      var issue = new Issue(GuidGenerator.Create(), repositoryId, title, text);

      if (await _issueRepository.AnyAsync(issue => issue.Title == title))
      {
        throw new BusinessException("IssueTracking:IssueWithSameTitleExists");
      }
      return new Issue(GuidGenerator.Create(), repositoryId, title, text);
    }

    public async Task ChangeTitleAsync(Issue issue, string title)
    {
       if (issue.Title == title)
       {
           return;
       }

       if (await _issueRepository.AnyAsync(issue => issue.Title == title))
       {
           throw new BusinessException("IssueTracking:IssueWithSameTitleExists");
       }

      issue.SetTitle(title);
    }
  }
}