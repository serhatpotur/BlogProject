using BlogApp.Business.Abstracts;
using BlogApp.Entities.DTOs.Roles;
using BlogApp.Entities.Entities;
using BlogApp.Web.ToastMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace BlogApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IToastNotification _toast;

        public RolesController(IToastNotification toast, IRoleService roleService)
        {
            _toast = toast;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AssignRoleToUser(string userId)
        {
            ViewBag.userId = userId;
            var userRolesList = await _roleService.AssignRoleToUser(userId);
            return View(userRolesList);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoleToUser(string userId, List<AssignRoleToUserDto> assignRoles)
        {
            var (userName, userRolesText) = await _roleService.UserRolesChange(userId, assignRoles);
            _toast.AddSuccessToastMessage(Messages.Role.Update(userName, userRolesText));
            return RedirectToAction("Index", "User", new { Area = "Admin" });
        }
    }
}
