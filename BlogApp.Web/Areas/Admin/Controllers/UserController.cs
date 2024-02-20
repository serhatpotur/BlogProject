using AutoMapper;
using BlogApp.Business.Abstracts;
using BlogApp.Business.Helpers.Images;
using BlogApp.DataAccess.UnitOfWorks.Abstracts;
using BlogApp.Entities.DTOs.Users;
using BlogApp.Entities.Entities;
using BlogApp.Web.ToastMessages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace BlogApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IToastNotification _toast;
        private readonly IAppUserService _appUserService;

        public UserController(UserManager<AppUser> userManager, IToastNotification toast, IAppUserService appUserService)
        {
            _userManager = userManager;
            _toast = toast;
            _appUserService = appUserService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _appUserService.GetAllUsersAsync();
            return View(users);
        }


        public async Task<IActionResult> Delete(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                _toast.AddSuccessToastMessage(Messages.User.Delete(user.UserName), new ToastrOptions { Title = "Silme İşlemi Başarılı!" });
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            _toast.AddErrorToastMessage(result.Errors.First().Description, new ToastrOptions { Title = "Silme İşlemi Başarısız!" });
            return RedirectToAction("Index", "User", new { Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var appUserProfileDto = await _appUserService.GetUserAsync(user);
            return View(appUserProfileDto);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(AppUserProfileDto userProfileDto)
        {
            if (!ModelState.IsValid)
            {
                _toast.AddErrorToastMessage("Bilgiler Güncellenemedi", new ToastrOptions { Title = "Hata!" });
                return View();
            }

            var updateResult = await _appUserService.UpdateUserAsync(userProfileDto);

            if (!updateResult.Succeeded)
            {
                _toast.AddErrorToastMessage("Bilgiler Güncellenemedi", new ToastrOptions { Title = "Hata!" });
                return View();
            }
            _toast.AddSuccessToastMessage("Bilgiler Başarılı Bir Şekilde Güncellendi", new ToastrOptions { Title = "Başarılı!" });
            return RedirectToAction("Profile", "User", new { Area = "Admin" });
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(AppUserChangePasswordDto passwordChangeDto)
        {
            var (isSuccess, message) = await _appUserService.ChangeUserPasswordAsync(passwordChangeDto);
            if (!isSuccess)
            {
                _toast.AddErrorToastMessage(message, new ToastrOptions { Title = "Hata!" });
                return View();
            }
            _toast.AddSuccessToastMessage(message, new ToastrOptions { Title = "Başarılı!" });
            return View();
           
        }

    }
}
