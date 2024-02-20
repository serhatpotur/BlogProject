using AutoMapper;
using BlogApp.Business.Abstracts;
using BlogApp.Business.Extensions;
using BlogApp.DataAccess.UnitOfWorks.Abstracts;
using BlogApp.Entities.DTOs.Users;
using BlogApp.Entities.Entities;
using BlogApp.Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Business.Concretes
{
    public class AppUserManager : IAppUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _context;
        private readonly ClaimsPrincipal _user;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public AppUserManager(UserManager<AppUser> userManager, IMapper mapper, IImageService imageService, IUnitOfWork unitOfWork, SignInManager<AppUser> signInManager, IHttpContextAccessor context)
        {
            _userManager = userManager;
            _mapper = mapper;
            _imageService = imageService;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _context = context;
            _user = _context.HttpContext.User;
        }

        public async Task<(bool, string)> ChangeUserPasswordAsync(AppUserChangePasswordDto appUserChangePasswordDto)
        {
            var user = await FindByUserIdAsync(_user.GetLoggedInUserId().ToString());
            var checkUser = await _userManager.CheckPasswordAsync(user, appUserChangePasswordDto.CurrentPassword);
            if (!checkUser)
            {
                return (false, "Mevcut Şifrenizi Yanlış Girdiniz");
            }
            if (appUserChangePasswordDto.NewPassword != appUserChangePasswordDto.ConfirmPassword)
            {
                return (false, "Girilen Yeni Şifreler Birbiriyle Uyuşmuyor");
            }
            var result = await _userManager.ChangePasswordAsync(user, appUserChangePasswordDto.CurrentPassword, appUserChangePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return (false, result.Errors.First().Description);
            }
            await _userManager.UpdateSecurityStampAsync(user);
            await _signInManager.SignOutAsync();
            await _signInManager.PasswordSignInAsync(user, appUserChangePasswordDto.NewPassword, true, false);

            return (true, "Şifreniz başarılı bir şekilde değiştirilmiştir");
        }

        public async Task<AppUser> FindByUserIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user;

        }

        public async Task<List<AppUserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDto = _mapper.Map<List<AppUserDto>>(users);
            return userDto;
        }

        public async Task<AppUserProfileDto> GetUserAsync(AppUser user)
        {
            var getImageByUser = await _unitOfWork.GetRepository<AppUser>().GetAsync(x => x.Id == user.Id, x => x.Image);

            var profileDto = _mapper.Map<AppUserProfileDto>(user);
            if (getImageByUser.Image != null)
                profileDto.Image.FileName = getImageByUser.Image.FileName;

            return profileDto;
        }

        public async Task<IdentityResult> UpdateUserAsync(AppUserProfileDto userProfileDto)
        {
            AppUser user = await FindByUserIdAsync(userProfileDto.Id.ToString());

            var image = await _imageService.AddImageAsync(userProfileDto.FormFile, userProfileDto.Username,ImageType.User);
            if (image != null)
                user.ImageId = image.Id;

            user.FirstName = userProfileDto.FirstName;
            user.LastName = userProfileDto.LastName;
            user.Email = userProfileDto.Email;
            user.PhoneNumber = userProfileDto.PhoneNumber;
            user.UserName = userProfileDto.Username;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                await _userManager.UpdateSecurityStampAsync(user);


            return result;
        }

        public async Task<(bool, string)> UserLoginAsync(AppUserLoginDto appUserLoginDto)
        {
            var user = await _userManager.FindByEmailAsync(appUserLoginDto.Email);
            if (user == null)
                return (false, "Kullanıcı Bulunamadı");

            var result = await _signInManager.PasswordSignInAsync(user, appUserLoginDto.Password, appUserLoginDto.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    return (false, "Şifrenizi 3 kez yanlış girdiğiniz için sisteme 3 dakila boyunca giriş yapamazsınız");

                return (false, $"Email adresiniz veya şifre hatalı. Başarısız giriş sayısı : {await _userManager.GetAccessFailedCountAsync(user)}");
            }
            return (true, string.Empty);




        }

        public async Task UserSignOutAsync()
        {
            await _signInManager.SignOutAsync();

        }
    }
}
