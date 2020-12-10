using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using IssueTracking.Domain.Shared.Issues;
using Volo.Abp;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Timing;

namespace IssueTracking.Domain.Issues
{
  public class Issue : AggregateRoot<Guid>, IHasCreationTime
  {
    public Guid RepositoryId { get; private set; }
    public string Title { get; private set; }
    public string Text { get; private set; }
    public bool IsClosed { get; private set; }
    public Guid? AssignedUserId { get; internal set; }
    public bool IsLocked { get; private set; }
    public IssueCloseReason? CloseReason { get; private set; }
    public ICollection<IssueLabel> Labels { get; set; }
    public DateTime CreationTime { get; private set; }
    public DateTime? LastCommentTime { get; private set; }
    public DateTime? CloseDate { get; set; }
    public Guid MileStoneId { get; set; }

    private Issue() { }

    internal Issue(Guid id, Guid repositoryId, string title, string text = null, Guid? assignedUserId = null) : base(id)
    {
      RepositoryId = repositoryId;
      Title = Check.NotNullOrWhiteSpace(title, nameof(title));
      Text = text;
      AssignedUserId = assignedUserId;
      Labels = new Collection<IssueLabel>();
    }

    internal void SetTitle(string title)
    {
      this.Title = Check.NotNullOrWhiteSpace(title, nameof(title));
    }

    public void Close(IssueCloseReason reason)
    {
      IsClosed = true;
      CloseReason = reason;
      CloseDate = DateTime.Now;
    }

    public void Reopen()
    {
      if (IsLocked)
      {
        throw new IssueStateException("IssueTracking:CanNotOpenLockedIssue");
      }
      IsClosed = false;
      CloseReason = null;
      CloseDate = null;
    }

    public void Lock()
    {
      if (!IsClosed)
      {
        throw new IssueStateException("IssueTracking:CanNotLockAnOpenIssue");
      }
      IsLocked = true;
    }

    public void Unlock()
    {
      IsLocked = false;
    }

    public void AddComment(Guid guid, string text)
    {
      throw new NotImplementedException();
    }

    // public async Task AssignToAsync(AppUser user, IUserIssueService userIssueService)   
    // {
    //   var openIssueCount = await userIssueService.GetOpenIssueCountAsync(user.Id);


    // }

    public bool IsInActive()
    {
      return new InActiveIssueSpecification().IsSatisfiedBy(this); 
    }
    


  }
}