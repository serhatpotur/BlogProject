using BlogApp.Entities.DTOs.Categories;
using BlogApp.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Entities.DTOs.Articles
{
    public class ArticleDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ViewCount { get; set; }
        public string Content { get; set; }
        public Guid CategoryId { get; set; }
        public IList<GetListCategoryDto> Categories { get; set; }

        public Guid? ImageId { get; set; }
        public Image Image { get; set; }
    }
}
