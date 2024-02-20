using AutoMapper;
using BlogApp.Business.Abstracts;
using BlogApp.Business.Extensions;
using BlogApp.DataAccess.UnitOfWorks.Abstracts;
using BlogApp.Entities.DTOs.Categories;
using BlogApp.Entities.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Business.Concretes
{
    public class CategoryManager : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ClaimsPrincipal _user;


        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContext = null)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContext = httpContext;
            _user = _httpContext.HttpContext.User;
        }

        public async Task AddAsync(AddCategoryDto categoryDto)
        {
            var username = _user.GetLoggedInUsername();
            categoryDto.CreatedBy = username;
            categoryDto.CreatedDate = DateTime.Now;
            var category = _mapper.Map<Category>(categoryDto);
            await _unitOfWork.GetRepository<Category>().AddAsync(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<GetListCategoryDto>> GetAllCategoriesNotDeletedAsync()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(x => !x.isDeleted,x=>x.Articles);
            var mappedCategories = _mapper.Map<List<GetListCategoryDto>>(categories);
            return mappedCategories;
        }

        public async Task<CategoryDto> GetByIdAsync(Guid id)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            var categoryDto = _mapper.Map<CategoryDto>(category);
            return categoryDto;
        }

        public async Task<string> UpdateAsync(UpdateCategoryDto categoryDto)
        {
            var username = _user.GetLoggedInUsername();
            var category = await _unitOfWork.GetRepository<Category>().GetAsync(x => x.Id == categoryDto.Id && !x.isDeleted);

            category.Name = categoryDto.Name;
            category.ModifiedBy = username;
            category.ModifiedDate = DateTime.Now;

            await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await _unitOfWork.SaveAsync();

            return category.Name;


        }
        public async Task<string> SafeDeleteAsync(Guid id)
        {
            var username = _user.GetLoggedInUsername();
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);

            category.isDeleted = true;
            category.DeletedDate = DateTime.Now;
            category.DeletedBy = username;

            await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await _unitOfWork.SaveAsync();

            return category.Name;

        }

        public async Task<List<GetListCategoryDto>> GetAllCategoriesDeletedAsync()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllAsync(x => x.isDeleted);
            var mappedCategories = _mapper.Map<List<GetListCategoryDto>>(categories);
            return mappedCategories;
        }

        public async Task<string> UndoDeleteAsync(Guid id)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);

            category.isDeleted = false;
            category.DeletedDate = null;
            category.DeletedBy = null;

            await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await _unitOfWork.SaveAsync();

            return category.Name;
        }
    }
}
