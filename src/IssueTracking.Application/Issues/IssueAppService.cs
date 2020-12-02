using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using IssueTracking.Application.Contracts.Issues;
using IssueTracking.Domain.Issues;
using IssueTracking.Domain.Users;
using IssueTracking.Users;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Specifications;

namespace IssueTracking.Application.Issues
{
  public class IssueAppService : ApplicationService, IIssueAppService
  {
    private readonly IssueManager _issueManager;
    private IRepository<Issue, Guid> _issueRepository;
    private readonly IRepository<AppUser, Guid> _userRepository;

    public IssueAppService(IssueManager issueManager, IRepository<Issue, Guid> issueRepository, IRepository<AppUser, Guid> appuserRepository)
    {
      _issueManager = issueManager;
      _issueRepository = issueRepository;
      _userRepository = appuserRepository;
    }

    [Authorize]
    public async Task CreateCommentAsync(CreateCommentDto input)
    {
      var issue = await _issueRepository.GetAsync(input.IssueId);
      issue.AddComment(CurrentUser.GetId(), input.Text);
      await _issueRepository.UpdateAsync(issue);
    }

    public async Task DoItAsync(Guid mileStoneId)
    {

      var issues = await AsyncExecuter.ToListAsync(
        _issueRepository
          .Where(
              new InActiveIssueSpecification()
                .And(new MileStoneSpecification(mileStoneId)).ToExpression()
            ));

    }

    public async Task AssignToAsync(IssueAssignDto input)
    {
      var issue = await _issueRepository.GetAsync(input.IssueId);
      var user = await _userRepository.GetAsync(input.UserId);

      await _issueManager.AssignToAsync(issue, user);
      await _issueRepository.UpdateAsync(issue);
    }

    public async Task<IssueDto> CreateAsync(CreateIssueDto input)
    {
      var issue = await _issueManager.CreateAsync(input.RepositoryId, input.Title, input.Text);

      // Apply additional domain actions
      if (input.AssignedUserId.HasValue)
      {
        var user = await _userRepository.GetAsync(input.AssignedUserId.Value);
        await _issueManager.AssignToAsync(issue, user);
      }

      // Save
      await _issueRepository.InsertAsync(issue);

      return ObjectMapper.Map<Issue, IssueDto>(issue);
    }

    public async Task UpdateAsync(Guid id, UpdateIssueDto input)
    {

      var issue = await _issueRepository.GetAsync(id);
      await _issueManager.ChangeTitleAsync(issue, input.Title);
    }

    public async Task<IssueDto> GetAsync(Guid id)
    {
      var issue = await _issueRepository.GetAsync(id);
      return ObjectMapper.Map<Issue, IssueDto>(issue);
    }

    public async Task<PagedResultDto<IssueDto>> GetListAsync(GetIssueListDto input)
    {
      if (input.Sorting.IsNullOrWhiteSpace())
      {
        input.Sorting = nameof(Issue.Title);
      }
      var issues = await _issueRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);

      var totalCount = await AsyncExecuter.CountAsync(_issueRepository.WhereIf(!input.Filter.IsNullOrWhiteSpace(), issue => issue.Title.Contains(input.Filter)));

      return new PagedResultDto<IssueDto>(totalCount, ObjectMapper.Map<List<Issue>, List<IssueDto>>(issues));
    }

    public async Task DeleteAsync(Guid id)
    {
      await _issueRepository.DeleteAsync(id);
    }
  }

  // public class IssueAppService : ApplicationService, IIssueAppService
  // {

  //   private readonly IIssueRepository _issueRepository;
  //   //private IRepository<Issue, Guid> _issueRepository;

  //    public IssueAppService(IIssueRepository issueRepository)
  //   //public IssueAppService(IRepository<Issue, Guid> issueRepository)
  //   {
  //     _issueRepository = issueRepository;
  //   }

  //   [Authorize]
  //   public async Task CreateCommentAsync(CreateCommentDto input)
  //   {
  //     var issue = await _issueRepository.GetAsync(input.IssueId);
  //     issue.AddComment(CurrentUser.GetId(), input.Text);
  //     await _issueRepository.UpdateAsync(issue);
  //   }

  //   public async Task DoItAsync()
  //   {
  //     var issues = await _issueRepository.GetIssuesAsync(new InActiveIssueSpecification());

  //     //var issues = await AsyncExecuter.ToListAsync(_issueRepository.Where(new InActiveIssueSpecification()));

  //   }
  // }


}