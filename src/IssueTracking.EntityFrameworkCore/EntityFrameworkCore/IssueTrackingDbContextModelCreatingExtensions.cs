using IssueTracking.Domain.Issues;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace IssueTracking.EntityFrameworkCore
{
  public static class IssueTrackingDbContextModelCreatingExtensions
  {
    public static void ConfigureIssueTracking(this ModelBuilder builder)
    {
      Check.NotNull(builder, nameof(builder));

      /* Configure your own tables/entities inside here */

      //builder.Entity<YourEntity>(b =>
      //{
      //    b.ToTable(IssueTrackingConsts.DbTablePrefix + "YourEntities", IssueTrackingConsts.DbSchema);
      //    b.ConfigureByConvention(); //auto configure for the base class props
      //    //...
      //});
      builder.Entity<Issue>(b =>
      {
        b.ToTable(IssueTrackingConsts.DbTablePrefix + "Issues", IssueTrackingConsts.DbSchema);
        b.ConfigureByConvention();

        //b.Property(x => x.Name).IsRequired().HasMaxLength(IssueConsts.MaxNameLength);
        // b.HasIndex(x => x.Name);
      });


      builder.Entity<IssueLabel>(b =>
      {
        b.HasKey(e => new { e.LabelId, e.IssueId }).IsClustered(false);
        b.ToTable(IssueTrackingConsts.DbTablePrefix + "IssueLabels", IssueTrackingConsts.DbSchema);
        b.ConfigureByConvention();

        //b.Property(x => x.Name).IsRequired().HasMaxLength(IssueLabelConsts.MaxNameLength);
        // b.HasIndex(x => x.Name);
      });
    }
  }
}