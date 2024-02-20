using BlogApp.Business.Abstracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Web.ViewComponents
{
    public class RecentPostViewComponent : ViewComponent
    {
        private readonly IArticleService _articleService;

        public RecentPostViewComponent(IArticleService articleService)
        {
            _articleService = articleService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var articles = await _articleService.GetLastThreePosts();
            return View(articles);
        }
    }
}
