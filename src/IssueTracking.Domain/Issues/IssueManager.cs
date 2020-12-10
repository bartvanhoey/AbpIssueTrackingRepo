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
    public const int MaxAllowedOpenIssuesForAUser = 3;
    private readonly IIssueRepository _issueRepository;

    public IssueManager(IIssueRepository issueRepository)
    {
      _issueRepository = issueRepository;
    }

    public async Task AssignToAsync(Issue issue, AppUser user)
    {
      var openIssueCount = await _issueRepository.CountAsync(issue => issue.AssignedUserId == user.Id && !issue.IsClosed);

      if (openIssueCount >= MaxAllowedOpenIssuesForAUser)
      {
        throw new BusinessException(code: "IM:00392", message: $"You cannot assign more than {MaxAllowedOpenIssuesForAUser} issues to a user!");
      }
      issue.AssignedUserId = user.Id;
    }

 public async Task AssignToUserAsync(Issue issue, Guid userId)
    {
      var numberOpenIssues = await _issueRepository.GetIssueCountOfUserAsync(userId);

      if (numberOpenIssues >= MaxAllowedOpenIssuesForAUser)
      {
        throw new BusinessException(code: "IM:00392", message: $"You cannot assign more than {MaxAllowedOpenIssuesForAUser} issues to a user!");
      }
      issue.AssignedUserId = userId;
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