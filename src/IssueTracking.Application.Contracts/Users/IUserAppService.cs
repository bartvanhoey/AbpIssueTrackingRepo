using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using IssueTracking.Domain.Shared.Users;
using Volo.Abp.Application.Services;

namespace IssueTracking.Application.Contracts.Users
{
  public interface IUserAppService : IApplicationService
  {
    Task<UserDto> GetAsync(Guid id);
    Task<List<UserDto>> GetList();

    // Task<PagedResultDto<UserDto>> GetListAsync(GetUserListDto input);

    Task<UserDto> CreateAsync(CreateUserDto input);

    Task<UserDto> UpdateAsync(Guid id, UpdateUserDto input);

    Task DeleteAsync(Guid id);

  }

  public class UserDto
  {
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime CreationTime { get; set; }
    public List<string> Roles { get; set; }
  }

  public class ChangePasswordDto
  {
    public Guid Id { get; set; }
    public string Password { get; set; }
  }

  public class UpdateUserDto
  {
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
  }

  public class CreateUserDto
  {
    [Required]
    [StringLength(UserConsts.MaxUserNameLength)]
    public string UserName { get; set; }

    [Required]
    [StringLength(UserConsts.MaxEmailLength)]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(UserConsts.MaxEmailLength, MinimumLength = UserConsts.MinPasswordLength)]
    public string Password { get; set; }
  }
}