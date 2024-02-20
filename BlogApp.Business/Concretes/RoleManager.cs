using BlogApp.Business.Abstracts;
using BlogApp.Entities.DTOs.Roles;
using BlogApp.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Business.Concretes
{
    public class RoleManager : IRoleService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleManager(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<AssignRoleToUserDto>> AssignRoleToUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await _userManager.GetRolesAsync(user);
            var roleList = new List<AssignRoleToUserDto>();

            foreach (var role in roles)
            {
                var assignRoleToUser = new AssignRoleToUserDto()
                {
                    Id = role.Id,
                    Name = role.Name
                };

                if (userRoles.Contains(role.Name)) // kullanıcıda bu rol var
                {
                    assignRoleToUser.Exist = true;
                }

                roleList.Add(assignRoleToUser);
            }
            return roleList;
        }

        public async Task<(string,string)> UserRolesChange(string userId, List<AssignRoleToUserDto> assignRoles)
        {
            List<string> userNewRoles = new();
            var user = await _userManager.FindByIdAsync(userId);
            foreach (var role in assignRoles)
            {
                if (role.Exist)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                    userNewRoles.Add(role.Name);
                }

                else
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
            }
            string userRolesText = string.Join(", ", userNewRoles);
            return (user.UserName,userRolesText);
        }
    }
}
