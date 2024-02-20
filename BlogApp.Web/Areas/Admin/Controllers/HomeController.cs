using BlogApp.Business.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BlogApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IArticleService _article;
        private readonly IDashboardService _dashboardService;

        public HomeController(IArticleService article, IDashboardService dashboardService)
        {
            _article = article;
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await _article.GetAllArticlesWithCategoryNotDeletedAsync();
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> YearlyArticleCount()
        {
            var count = await _dashboardService.GetYearlyArticleCount();
            return Json(JsonSerializer.Serialize(count));
        }

        [HttpGet]
        public async Task<IActionResult> TotalArticleCount()
        {
            var count = await _dashboardService.GetTotalArticleCount();
            return Json(count);
        }
        [HttpGet]
        public async Task<IActionResult> TotalCategoryCount()
        {
            var count = await _dashboardService.GetTotalCategoryCount();
            return Json(count);
        }
    }
}
