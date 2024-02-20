using AutoMapper;
using BlogApp.Business.Extensions;
using BlogApp.Entities.DTOs.Users;
using BlogApp.Entities.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NToastNotify;
using System.Security.Claims;

namespace BlogApp.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toast;
        private readonly IValidator<AppUser> _validator;

        public AuthController(UserManager<AppUser> userManager, IMapper mapper, IToastNotification toast, RoleManager<AppRole> roleManager, IValidator<AppUser> validator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _toast = toast;
            _roleManager = roleManager;
            _validator = validator;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(AppUserRegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //var mappedUser = _mapper.Map<AppUser>(registerDto);
            AppUser user = new AppUser(registerDto.Username, registerDto.FirstName, registerDto.LastName, registerDto.Email, registerDto.PhoneNumber);
            var validationResult = await _validator.ValidateAsync(user);
            var result = await _userManager.CreateAsync(user, (String.IsNullOrEmpty(registerDto.Password) ? "" : registerDto.Password));
            if (!result.Succeeded)
            {
                _toast.AddErrorToastMessage(result.Errors.First().Description, new ToastrOptions { Title = "Kayıt Başarısız!" });

                result.AddToIdentityModelState(this.ModelState);

                validationResult.AddToModelState(this.ModelState);
                return View(registerDto);
            }
            var userRole = await _roleManager.FindByNameAsync("User");
            var roleResult = await _userManager.AddToRoleAsync(user, userRole.Name);
            if (!roleResult.Succeeded)
            {
                _toast.AddErrorToastMessage(roleResult.Errors.First().Description, new ToastrOptions { Title = "Kayıt Başarısız!" });

                roleResult.AddToIdentityModelState(this.ModelState);

                validationResult.AddToModelState(this.ModelState);
                return View(registerDto);
            }

            _toast.AddSuccessToastMessage("Kayıt Olma İşlemi Başarılı", new ToastrOptions { Title = "Kayıt Başarılı!" });

            return RedirectToAction("Index", "Home");

        }
    }
}
