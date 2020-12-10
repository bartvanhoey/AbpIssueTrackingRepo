using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IssueTracking.Domain.Issues;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Specifications;

namespace IssueTracking.EntityFrameworkCore.Issues
{

  public class EfCoreIssueRepository : EfCoreRepository<IssueTrackingDbContext, Issue, Guid>, IIssueRepository
  {
    public EfCoreIssueRepository(IDbContextProvider<IssueTrackingDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public async Task<int> GetIssueCountOfUserAsync(Guid userId)
    {
      return await DbSet.Where(i => i.AssignedUserId == userId && i.IsClosed == false).CountAsync();
    }

    public async Task<List<Issue>> GetIssuesAsync(ISpecification<Issue> spec)
    {
      return await DbSet.Where(spec.ToExpression()).ToListAsync();
    }

    
  }


}