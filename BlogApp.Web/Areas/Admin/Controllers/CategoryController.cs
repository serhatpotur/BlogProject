using AutoMapper;
using BlogApp.Business.Abstracts;
using BlogApp.Business.Extensions;
using BlogApp.Entities.DTOs.Categories;
using BlogApp.Entities.Entities;
using BlogApp.Web.ToastMessages;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace BlogApp.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<Category> _validator;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toast;

        public CategoryController(ICategoryService categoryService, IValidator<Category> validator, IMapper mapper, IToastNotification toast)
        {
            _categoryService = categoryService;
            _validator = validator;
            _mapper = mapper;
            _toast = toast;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesNotDeletedAsync();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryDto addCategory)
        {

            var category = _mapper.Map<Category>(addCategory);
            var result = await _validator.ValidateAsync(category);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View(addCategory);
            }
            await _categoryService.AddAsync(addCategory);
            _toast.AddSuccessToastMessage(Messages.Category.Add(addCategory.Name), new ToastrOptions { Title = "Ekleme Başarılı!" });
            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> AddWithAjax([FromBody] AddCategoryDto addCategory)
        {
            var category = _mapper.Map<Category>(addCategory);
            var result = await _validator.ValidateAsync(category);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                _toast.AddErrorToastMessage(result.Errors.First().ErrorMessage, new ToastrOptions { Title = "Ekleme Başarısız!" });
                return Json(result.Errors.First().ErrorMessage);
            }
            await _categoryService.AddAsync(addCategory);
            _toast.AddSuccessToastMessage(Messages.Category.Add(addCategory.Name), new ToastrOptions { Title = "Ekleme Başarılı!" });
            return Json(Messages.Category.Add(addCategory.Name));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var categoryDto = await _categoryService.GetByIdAsync(id);
            var updateCategoryDto = _mapper.Map<UpdateCategoryDto>(categoryDto);
            return View(updateCategoryDto);

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryDto updateCategory)
        {
            var category = _mapper.Map<Category>(updateCategory);
            var result = await _validator.ValidateAsync(category);
            if (!result.IsValid)
            {
                result.AddToModelState(this.ModelState);
                return View();
            }
            var updatedCategory = await _categoryService.UpdateAsync(updateCategory);
            _toast.AddSuccessToastMessage(Messages.Category.Update(updatedCategory), new ToastrOptions { Title = "Güncelleme Başarılı!" });
            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }
        public async Task<IActionResult> Delete(Guid id)
        {
            var deletedCategory = await _categoryService.SafeDeleteAsync(id);
            _toast.AddSuccessToastMessage(Messages.Category.Delete(deletedCategory), new ToastrOptions { Title = "Silme Başarılı" });
            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }

        public async Task<IActionResult> DeletedCategory()
        {
            var categories = await _categoryService.GetAllCategoriesDeletedAsync();
            return View(categories);
        }

        public async Task<IActionResult> UndoDelete(Guid id)
        {
            var deletedCategory = await _categoryService.UndoDeleteAsync(id);
            _toast.AddSuccessToastMessage(Messages.Category.UndoDelete(deletedCategory), new ToastrOptions { Title = "Geri Alma Başarılı" });
            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }
    }
}
