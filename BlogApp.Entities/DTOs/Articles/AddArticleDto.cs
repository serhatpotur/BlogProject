using BlogApp.Entities.DTOs.Categories;
using BlogApp.Entities.DTOs.Users;
using BlogApp.Entities.Entities;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Entities.DTOs.Articles
{
    public class AddArticleDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid CategoryId { get; set; }

        public IFormFile? FormFile { get; set; }
        public string? ImageId { get; set; }

        public Image? Image { get; set; }
        public IList<GetListCategoryDto> Categories { get; set; }
    }
}
