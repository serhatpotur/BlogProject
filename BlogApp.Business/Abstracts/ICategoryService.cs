using BlogApp.Entities.DTOs.Categories;

namespace BlogApp.Business.Abstracts
{
    public interface ICategoryService
    {
        Task<List<GetListCategoryDto>> GetAllCategoriesNotDeletedAsync();
        Task<List<GetListCategoryDto>> GetAllCategoriesDeletedAsync();
        Task AddAsync(AddCategoryDto categoryDto);
        Task<CategoryDto> GetByIdAsync(Guid id);
        Task<string> UpdateAsync(UpdateCategoryDto categoryDto);
        Task<string> SafeDeleteAsync(Guid id);
        Task<string> UndoDeleteAsync(Guid id);
    }
}
