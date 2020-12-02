// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Blazorise;
// using Blazorise.DataGrid;
// using IssueTracking.Application.Contracts.Issues;
// using Volo.Abp.Application.Dtos;

// namespace IssueTracking.Blazor.Pages
// {
//   public partial class Issues
//   {
//     protected IReadOnlyList<IssueDto> IssueList { get; set; }
    
//     protected int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
//     protected int CurrentPage { get; set; }
//     protected string CurrentSorting { get; set; }
//     protected int TotalCount { get; set; }

//     protected bool CanCreateIssue = true;
//     protected bool CanUpdateIssue = true;
//     protected bool CanDeleteIssue = true;

//     protected CreateIssueDto NewIssue { get; set; } = new CreateIssueDto();

//     protected Guid EditingIssueId { get; set; }
//     protected UpdateIssueDto EditingIssue { get; set; } = new UpdateIssueDto();

//     protected Modal CreateModal { get; set; }
//     protected Modal EditModal { get; set; }

//     protected override async Task OnInitializedAsync()
//     {
//       await SetPermissionsAsync();
//       await GetIssuesAsync();

//     }

//     protected async Task SetPermissionsAsync()
//     {
//       // CanCreateIssue = await AuthorizationService.IsGrantedAsync(IssueTrackingPermissions.Issue.Create);
//       // CanUpdateIssue = await AuthorizationService.IsGrantedAsync(IssueTrackingPermissions.Issue.Update);
//       // CanDeleteIssue = await AuthorizationService.IsGrantedAsync(IssueTrackingPermissions.Issue.Delete);
//     }

//     protected void OpenCreateModal()
//     {
//       NewIssue = new CreateIssueDto();
//       CreateModal.Show();
//     }

//     protected void CloseCreateModalAsync()
//     {
//       CreateModal.Hide();
//     }

//     protected void OpenEditModal(IssueDto issue)
//     {
//       EditingIssueId = issue.Id;
//       EditingIssue = ObjectMapper.Map<IssueDto, UpdateIssueDto>(issue);
//       EditModal.Show();
//     }

//     protected async Task DeleteIssueAsync(IssueDto issue)
//     {
//       var confirmMessage = L["IssueDeletionConfirmationMessage", issue.Title];
//       if (!await Message.Confirm(confirmMessage))
//       {
//         return;
//       }

//       await IssueAppService.DeleteAsync(issue.Id);
//       await GetIssuesAsync();
//     }

//     protected async Task GetIssuesAsync()
//     {
//       var result = await IssueAppService.GetListAsync(
//           new GetIssueListDto
//           {
//             MaxResultCount = PageSize,
//             SkipCount = CurrentPage * PageSize,
//             Sorting = CurrentSorting
//           }
//       );

//       IssueList = result.Items;
//       TotalCount = (int)result.TotalCount;
//     }

//     protected async Task OnDataGridReadAsync(DataGridReadDataEventArgs<IssueDto> e)
//     {
//       CurrentSorting = e.Columns
//           .Where(c => c.Direction != SortDirection.None)
//       .Select(c => c.Field + (c.Direction == SortDirection.Descending ? "DESC" : ""))
//       .JoinAsString(",");
//       CurrentPage = e.Page - 1;

//       await GetIssuesAsync();

//       StateHasChanged();
//     }

//     protected void CloseEditModalAsync()
//     {
//       EditModal.Hide();
//     }

//     protected async Task CreateEntityAsync()
//     {
//       await IssueAppService.CreateAsync(NewIssue);
//       await GetIssuesAsync();
//       CreateModal.Hide();
//     }

//     protected async Task UpdateEntityAsync()
//     {
//       await IssueAppService.UpdateAsync(EditingIssueId, EditingIssue);
//       await GetIssuesAsync();
//       EditModal.Hide();
//     }

//   }
// }