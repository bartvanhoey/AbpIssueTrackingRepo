using System;

namespace IssueTracking.Application.Contracts.Issues
{
  public class IssueAssignDto
  {
    public Guid IssueId { get; set; }
    public Guid UserId { get; set; }
  }
}