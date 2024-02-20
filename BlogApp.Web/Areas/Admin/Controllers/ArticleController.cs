using AutoMapper;
using BlogApp.Business.Abstracts;
using BlogApp.Business.Extensions;
using BlogApp.Entities.DTOs.Articles;
using BlogApp.Entities.Entities;
using BlogApp.Web.Consts;
using BlogApp.Web.ToastMessages;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace BlogApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<Article> _validator;
        private readonly IToastNotification _toast;


        public ArticleController(IArticleService articleService, ICategoryService categoryService, IMapper mapper, IValidator<Article> validator, IToastNotification toast)
        {
            _articleService = articleService;
            _categoryService = categoryService;
            _mapper = mapper;
            _validator = validator;
            _toast = toast;
        }
        public async Task<IActionResult> Index()
        {
            var articles = await _articleService.GetAllArticlesWithCategoryNotDeletedAsync();
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryService.GetAllCategoriesNotDeletedAsync();
            var addArticleDto = new AddArticleDto()
            {
                Categories = categories
            };


            return View(addArticleDto);
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddArticleDto articleDto)
        {
            var mappedArticle = _mapper.Map<Article>(articleDto);
            ValidationResult result = await _validator.ValidateAsync(mappedArticle);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                var categories = await _categoryService.GetAllCategoriesNotDeletedAsync();
                var addArticleDto = new AddArticleDto() { Categories = categories };
                return View(addArticleDto);
            }
            await _articleService.AddAsync(articleDto);
            _toast.AddSuccessToastMessage(Messages.Article.Add(articleDto.Title), new ToastrOptions { Title = "Ekleme Başarılı!" });
            return RedirectToAction("Index", "Article", new { Area = "Admin" });

        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var article = await _articleService.GetByIdAsync(id);


            var categories = await _categoryService.GetAllCategoriesNotDeletedAsync();
            var updateArticleDto = _mapper.Map<UpdateArticleDto>(article);
            updateArticleDto.Categories = categories;


            return View(updateArticleDto);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateArticleDto articleDto)
        {
            var mappedArticle = _mapper.Map<Article>(articleDto);

            ValidationResult result = await _validator.ValidateAsync(mappedArticle);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                var categories = await _categoryService.GetAllCategoriesNotDeletedAsync();
                articleDto.Categories = categories;
                return View(articleDto);

            }
            var updatedArticle = await _articleService.UpdateAsync(articleDto);
            _toast.AddSuccessToastMessage(Messages.Article.Update(updatedArticle), new ToastrOptions { Title = "Güncelleme Başarılı!" });

            return RedirectToAction("Index", "Article", new { Area = "Admin" });


        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedArticle = await _articleService.SafeDeleteAsync(id);
            _toast.AddSuccessToastMessage(Messages.Article.Delete(deletedArticle), new ToastrOptions { Title = "Silme Başarılı!" });

            return RedirectToAction("Index", "Article", new { Area = "Admin" });


        }

        [Authorize(Roles = $"{RoleConst.Admin},{RoleConst.Mod}")]
        public async Task<IActionResult> DeletedArticle()
        {
            var articles = await _articleService.GetAllDeletedArticles();
            return View(articles);
        }

        [Authorize(Roles = $"{RoleConst.Admin},{RoleConst.Mod}")]

        public async Task<IActionResult> UndoDelete(Guid id)
        {
            var deletedArticle = await _articleService.UndoDeleteAsync(id);
            _toast.AddSuccessToastMessage(Messages.Article.UndoDelete(deletedArticle), new ToastrOptions { Title = "Geri Alma Başarılı!" });

            return RedirectToAction("Index", "Article", new { Area = "Admin" });


        }
    }
}
