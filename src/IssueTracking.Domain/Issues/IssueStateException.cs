using System;
using System.Runtime.Serialization;
using Volo.Abp;

namespace IssueTracking.Domain.Issues
{
  
  public class IssueStateException  : BusinessException
  {
    //base(IssueTrackingDomainErrorCodes.IssueState)
    public IssueStateException(string message) : base(message) 
    {
      // WithData("name", name);
    }
  }
}