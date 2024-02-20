using BlogApp.Entities.DTOs.Users;
using BlogApp.Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BlogApp.Business.Abstracts
{
    public interface IAppUserService
    {
        Task<AppUserProfileDto> GetUserAsync(AppUser user);
        Task<List<AppUserDto>> GetAllUsersAsync();
        Task<AppUser> FindByUserIdAsync(string userId);
        Task<IdentityResult> UpdateUserAsync(AppUserProfileDto userProfileDto);
        Task<(bool, string)>ChangeUserPasswordAsync(AppUserChangePasswordDto appUserChangePasswordDto);
        Task<(bool, string)> UserLoginAsync(AppUserLoginDto appUserLoginDto);
        Task UserSignOutAsync();
    }
}
