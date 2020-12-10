using System;
using System.Threading.Tasks;
using IssueTracking.Domain.Issues;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;

namespace IssueTracking
{
  public class IssueTrackingTestDataSeedContributor : IDataSeedContributor, ITransientDependency
  {
    private readonly IIssueRepository _issueRepository;
    private readonly IssueManager _issueManager;

    public IssueTrackingTestDataSeedContributor(IIssueRepository issueRepository)
    {
      _issueRepository = issueRepository;
      _issueManager = new IssueManager(_issueRepository);
    }


    public async Task SeedAsync(DataSeedContext context)
    {
      var issue1 = await _issueManager.CreateAsync(Guid.NewGuid(), "Issue1Title", "Issue1Text");
      issue1.AssignedUserId = TestData.User1Id;
      await _issueRepository.InsertAsync(issue1);

      var issue2 = await _issueManager.CreateAsync(Guid.NewGuid(), "Issue2Title", "Issue2Text");
      issue2.AssignedUserId = TestData.User1Id;
            await _issueRepository.InsertAsync(issue2);

      var issue3 = await _issueManager.CreateAsync(Guid.NewGuid(), "Issue3Title", "Issue3Text");
      issue3.AssignedUserId = TestData.User2Id;
      
      await _issueRepository.InsertAsync(issue3);

      var issue4 = await _issueManager.CreateAsync(Guid.NewGuid(), "Issue3Title", "Issue3Text");
      issue4.AssignedUserId = TestData.User2Id;
      await _issueRepository.InsertAsync(issue4);

    }
  }


  public static class TestData
  {
    public static Guid User1Id = Guid.Parse("41951813-5CF9-4204-8B18-CD765DBCBC9B");
    public static Guid User2Id = Guid.Parse("2DAB4460-C21B-4925-BF41-A52750A9B999");
  }

}