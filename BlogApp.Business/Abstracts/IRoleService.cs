using BlogApp.Entities.DTOs.Roles;

namespace BlogApp.Business.Abstracts
{
    public interface IRoleService
    {
        Task<List<AssignRoleToUserDto>> AssignRoleToUser(string userId);
        Task<(string,string)> UserRolesChange(string userId, List<AssignRoleToUserDto> assignRoles);
    }
}
