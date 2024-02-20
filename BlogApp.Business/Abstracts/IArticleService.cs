using BlogApp.Entities.DTOs.Articles;
using BlogApp.Entities.Entities;

namespace BlogApp.Business.Abstracts
{
    public interface IArticleService
    {
        Task<List<GetListArticleDto>> GetAllArticlesWithCategoryNotDeletedAsync();
        Task<List<GetListArticleDto>> GetAllDeletedArticles();
        Task<List<GetListArticleDto>> GetLastThreePosts();
        Task AddAsync(AddArticleDto addArticle);
        Task<string> UpdateAsync(UpdateArticleDto updateArticle);
        Task<string> SafeDeleteAsync(Guid id);
        Task<string> UndoDeleteAsync(Guid id);
        Task<ArticleDto> GetByIdAsync(Guid id);

        Task<ArticleListDto> GetAllByPagingAsync(Guid? categoryId, string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false);
        Task<ArticleListDto> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false);
        Task<List<GetListArticleDto>> GetArticlesByCategoryId(Guid categoryId);
    }
}
