using AutoMapper;
using BlogApp.Business.Abstracts;
using BlogApp.Business.Extensions;
using BlogApp.DataAccess.UnitOfWorks.Abstracts;
using BlogApp.Entities.DTOs.Articles;
using BlogApp.Entities.Entities;
using BlogApp.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace BlogApp.Business.Concretes
{
    public class ArticleManager : IArticleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IImageService _ımageService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ClaimsPrincipal _user;

        public ArticleManager(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContext, IImageService ımageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContext = httpContext;
            _user = _httpContext.HttpContext.User;
            _ımageService = ımageService;
        }

        public async Task AddAsync(AddArticleDto addArticle)
        {
            var userId = _user.GetLoggedInUserId();
            var username = _user.GetLoggedInUsername();
            //var imageId = Guid.Parse("10661896-4D45-4104-BDAE-013454E227BB"); 
            var image = await _ımageService.AddImageAsync(addArticle.FormFile, username, ImageType.Article);
           

            var article = new Article(addArticle.Title, addArticle.Content, addArticle.CategoryId, image.Id, userId, username);

            await _unitOfWork.GetRepository<Article>().AddAsync(article);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<GetListArticleDto>> GetAllArticlesWithCategoryNotDeletedAsync()
        {
            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.isDeleted, x => x.Category);
            var mappedArticles = _mapper.Map<List<GetListArticleDto>>(articles);
            return mappedArticles;
        }

        public async Task<ArticleListDto> GetAllByPagingAsync(Guid? categoryId, string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {

            pageSize = pageSize > 20 ? 20 : pageSize;

            // Buradan 3 adet articles dönmesi gerekiyor ancak 0 geliyor . Yani linq da yanlış olan bir kısım var
            var articles = categoryId == null
                ? await _unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.isDeleted, x => x.Category, x => x.Image)
                : await _unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.isDeleted && x.CategoryId == categoryId, x => x.Category, x => x.Image);

            articles = keyword == null
                ? articles
                : await _unitOfWork.GetRepository<Article>().GetAllAsync((x => x.Title.Contains(keyword) || x.Content.Contains(keyword) || x.Category.Name.Contains(keyword)));

            var sortedArticles = isAscending
                ? articles.OrderBy(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new ArticleListDto
            {
                Articles = sortedArticles,
                CategoryId = categoryId == null ? null : categoryId.Value,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                isAscending = isAscending,
                Category = articles[0].Category
            };
        }

        public async Task<List<GetListArticleDto>> GetAllDeletedArticles()
        {
            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(x => x.isDeleted, x => x.Category);
            var mappedArticles = _mapper.Map<List<GetListArticleDto>>(articles);
            return mappedArticles;
        }

        public async Task<ArticleDto> GetByIdAsync(Guid id)
        {
            var article = await _unitOfWork.GetRepository<Article>().GetAsync(x => x.Id == id && !x.isDeleted, x => x.Category,x=>x.Image);
            var articleDto = _mapper.Map<ArticleDto>(article);
            return articleDto;
        }

        public async Task<List<GetListArticleDto>> GetLastThreePosts()
        {

            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.isDeleted, x => x.Category);
            articles = articles.OrderByDescending(x => x.CreatedDate).Take(3).ToList();
            var mappedArticles = _mapper.Map<List<GetListArticleDto>>(articles);
            return mappedArticles;
        }

        public async Task<string> SafeDeleteAsync(Guid id)
        {
            var username = _user.GetLoggedInUsername();

            var article = await _unitOfWork.GetRepository<Article>().GetByIdAsync(id);
            article.isDeleted = true;
            article.DeletedDate = DateTime.Now;
            article.DeletedBy = username;

            await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await _unitOfWork.SaveAsync();

            return article.Title;
        }

        public async Task<ArticleListDto> SearchAsync(string keyword, int currentPage = 1, int pageSize = 3, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;

            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.isDeleted && (x.Title.Contains(keyword) || x.Content.Contains(keyword) || x.Category.Name.Contains(keyword)), x => x.Category, x => x.Image);

            var sortedArticles = isAscending
                ? articles.OrderBy(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(x => x.CreatedDate).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new ArticleListDto
            {
                Articles = sortedArticles,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                isAscending = isAscending
            };
            throw new NotImplementedException();
        }

        public async Task<string> UndoDeleteAsync(Guid id)
        {
            var article = await _unitOfWork.GetRepository<Article>().GetByIdAsync(id);
            article.isDeleted = false;
            article.DeletedDate = null;
            article.DeletedBy = null;

            await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await _unitOfWork.SaveAsync();

            return article.Title;
        }

        public async Task<string> UpdateAsync(UpdateArticleDto updateArticle)
        {
            var username = _user.GetLoggedInUsername();
            var image = await _ımageService.AddImageAsync(updateArticle.FormFile, username, ImageType.Article);

            var article = await _unitOfWork.GetRepository<Article>().GetAsync(x => x.Id == updateArticle.Id && !x.isDeleted, x => x.Category);

            article.Title = updateArticle.Title;
            article.Content = updateArticle.Content;
            article.CategoryId = updateArticle.CategoryId;
            article.ModifiedDate = DateTime.Now;
            article.ModifiedBy = username;
            article.ImageId = image.Id;

            await _unitOfWork.GetRepository<Article>().UpdateAsync(article);
            await _unitOfWork.SaveAsync();

            return article.Title;



        }

        public async Task<List<GetListArticleDto>> GetArticlesByCategoryId(Guid categoryId)
        {
            var articles = await _unitOfWork.GetRepository<Article>().GetAllAsync(x => !x.isDeleted && x.CategoryId == categoryId);

            var mappedArticles = _mapper.Map<List<GetListArticleDto>>(articles);
            return mappedArticles;
        }
    }
}
