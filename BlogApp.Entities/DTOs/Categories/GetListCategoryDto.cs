using BlogApp.Entities.Entities;

namespace BlogApp.Entities.DTOs.Categories
{
    public class GetListCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
        public bool isDeleted { get; set; }

        public ICollection<Article> Articles { get; set; }

    }
}
