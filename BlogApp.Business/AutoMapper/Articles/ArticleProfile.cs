using AutoMapper;
using BlogApp.Entities.DTOs.Articles;
using BlogApp.Entities.Entities;

namespace BlogApp.Business.AutoMapper.Articles
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, GetListArticleDto>().ReverseMap();
            CreateMap<Article, UpdateArticleDto>().ReverseMap();
            CreateMap<Article, ArticleDto>().ReverseMap();
            CreateMap<Article, AddArticleDto>().ReverseMap();
            CreateMap<ArticleDto, UpdateArticleDto>().ReverseMap();
        }
    }
}
