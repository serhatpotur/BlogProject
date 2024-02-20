using AutoMapper;
using BlogApp.Entities.DTOs.Users;
using BlogApp.Entities.Entities;

namespace BlogApp.Business.AutoMapper.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<AppUser, AppUserRegisterDto>().ReverseMap();
            CreateMap<AppUser, AppUserProfileDto>().ReverseMap();
        }
    }
}
