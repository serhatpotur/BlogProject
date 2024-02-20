using BlogApp.Entities.Entities;

namespace BlogApp.Entities.DTOs.Articles
{
    public class ArticleListDto
    {
        public IList<Article> Articles { get; set; }
        public Guid? CategoryId { get; set; }
        public Category Category { get; set; }
        public virtual int CurrentPage { get; set; } = 1;
        public virtual int PageSize { get; set; } = 3;
        public virtual int TotalCount { get; set; }
        // ceiling : yukarı yuvarlar , divide : bölme
        public virtual int TotalPages => (int)Math.Ceiling(decimal.Divide(TotalCount, PageSize));

        public virtual bool isPrevious => CurrentPage > 1;
        public virtual bool isNext => CurrentPage < TotalPages;
        public virtual bool isAscending { get; set; } = false;
    }
}
