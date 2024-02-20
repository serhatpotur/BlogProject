using BlogApp.Business.Abstracts;
using BlogApp.Entities.DTOs.Users;
using BlogApp.Entities.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace BlogApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly IAppUserService _appUserService;

        public AuthController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AppUserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return View();

            var (isSuccess, message) = await _appUserService.UserLoginAsync(loginDto);
            if (!isSuccess)
            {
                ModelState.AddModelError("", message);
                return View();
            }
            return RedirectToAction("Index", "Home", new { Area = "Admin" });

            //var user = await _userManager.FindByEmailAsync(loginDto.Email);
            //if (user == null)
            //{
            //    ModelState.AddModelError("", "Kullanıcı Bulunamadı");
            //    return View();
            //}
            //var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, loginDto.RememberMe, true);
            //if (result.Succeeded)
            //    return RedirectToAction("Index", "Home", new { Area = "Admin" });

            //ModelState.AddModelError("", $"Email adresiniz veya şifre hatalı. Başarısız giriş sayısı : {await _userManager.GetAccessFailedCountAsync(user)}");
            //if (result.IsLockedOut)
            //{
            //    ModelState.AddModelError("", "Şifrenizi 3 kez yanlış girdiğiniz için sisteme 3 dakila boyunca giriş yapamazsınız");

            //    return View();
            //}

            //return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _appUserService.UserSignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
