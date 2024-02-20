using BlogApp.Business.Abstracts;
using BlogApp.DataAccess.UnitOfWorks.Abstracts;
using BlogApp.Entities.Entities;

namespace BlogApp.Business.Concretes
{
    public class DashboardManager : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DashboardManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<int>> GetYearlyArticleCount()
        {
            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.isDeleted);
            var startDate = DateTime.Now.Date;
            startDate = new DateTime(startDate.Year, 1, 1); // mevcut yılın ilk günü

            List<int> datas = new();

            for (int i = 1; i <= 12; i++)
            {
                var startedDate = new DateTime(startDate.Year, i, 1); // her ayın ilk günü
                var endedDate = startedDate.AddMonths(1);
                var data = articles.Where(x => x.CreatedDate >= startedDate && x.CreatedDate < endedDate).Count();
                datas.Add(data);

            }
            return datas;
        }

        public async Task<int> GetTotalArticleCount()
        {
            var count = await _unitOfWork.GetRepository<Article>().CountAsync();
            return count;

        }

        public async Task<int> GetTotalCategoryCount()
        {
            var count = await _unitOfWork.GetRepository<Category>().CountAsync();
            return count;

        }
    }
}
