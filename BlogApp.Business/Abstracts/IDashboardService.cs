namespace BlogApp.Business.Abstracts
{
    public interface IDashboardService
    {
        Task<List<int>> GetYearlyArticleCount();
        Task<int> GetTotalArticleCount();
        Task<int> GetTotalCategoryCount();
    }
}
