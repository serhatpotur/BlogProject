using BlogApp.Business.Abstracts;
using BlogApp.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace BlogApp.Web.Areas.Admin.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private UserManager<AppUser> _userManager;
        private readonly IAppUserService _appUserService;

        public NavbarViewComponent(UserManager<AppUser> userManager, IAppUserService appUserService)
        {
            _userManager = userManager;
            _appUserService = appUserService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var appUser = await _appUserService.GetUserAsync(user);
            return View(appUser);
        }
    }
}
