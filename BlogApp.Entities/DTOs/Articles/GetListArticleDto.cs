using BlogApp.Core.DTOs;
using BlogApp.Entities.DTOs.Categories;

namespace BlogApp.Entities.DTOs.Articles
{
    public class GetListArticleDto : IDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public GetListCategoryDto Category { get; set; }
        public  bool isDeleted { get; set; } 
        public  string CreatedBy { get; set; }
        public  DateTime CreatedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }



    }
}
