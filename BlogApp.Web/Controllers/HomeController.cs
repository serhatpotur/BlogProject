using BlogApp.Business.Abstracts;
using BlogApp.DataAccess.UnitOfWorks.Abstracts;
using BlogApp.Entities.Entities;
using BlogApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogApp.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IArticleService _articleService;
        private readonly IDashboardService _dashboardService;
        private readonly IHttpContextAccessor _context;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IArticleService articleService, IDashboardService dashboardService, IHttpContextAccessor context, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _articleService = articleService;
            _dashboardService = dashboardService;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid? categoryId, string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {

            var articles = await _articleService.GetAllByPagingAsync(categoryId, keyword, currentPage, pageSize, isAscending);

            return View(articles);

        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {

            var articles = await _articleService.SearchAsync(keyword, currentPage, pageSize, isAscending);

            return View(articles);

        }
        public async Task<IActionResult> Detail(Guid id)
        {
            var ipAdress = _context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            var articleVisitors = await _unitOfWork.GetRepository<ArticleVisitor>().GetAllAsync(null, x => x.Visitor, y => y.Article);
            var article = await _unitOfWork.GetRepository<Article>().GetAsync(x => x.Id == id);
            var result = await _articleService.GetByIdAsync(id);

            var visitor = await _unitOfWork.GetRepository<Visitor>().GetAsync(x => x.IpAdress == ipAdress);

            var addArticleVisitor = new ArticleVisitor()
            {
                ArticleId = article.Id,
                VisitorId = visitor.Id,
            };

            if (articleVisitors.Any(x => x.VisitorId == addArticleVisitor.VisitorId && x.ArticleId == addArticleVisitor.ArticleId))
                return View(result);
            else
            {
                await _unitOfWork.GetRepository<ArticleVisitor>().AddAsync(addArticleVisitor);
                article.ViewCount += 1;
                await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
                await _unitOfWork.SaveAsync();
            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetArticlesByCategoryId(Guid categoryId, string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {

            var articles = await _articleService.GetAllByPagingAsync(categoryId, keyword, currentPage, pageSize, isAscending);

            return View(articles);

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
