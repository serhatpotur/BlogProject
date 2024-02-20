using AutoMapper;
using BlogApp.Entities.DTOs.Articles;
using BlogApp.Entities.DTOs.Categories;
using BlogApp.Entities.Entities;

namespace BlogApp.Business.AutoMapper.Categories
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, GetListCategoryDto>().ReverseMap();
            CreateMap<Category, AddCategoryDto>().ReverseMap();
            CreateMap<Category, UpdateCategoryDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<UpdateCategoryDto, CategoryDto>().ReverseMap();
            CreateMap<UpdateCategoryDto, CategoryDto>().ReverseMap();
        }
    }
}
