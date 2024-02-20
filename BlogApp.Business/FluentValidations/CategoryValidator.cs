using BlogApp.Entities.Entities;
using FluentValidation;

namespace BlogApp.Business.FluentValidations
{
    public class CategoryValidator:AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().NotEmpty().MinimumLength(5).MaximumLength(50).WithName("Kategori Adı");

        }
    }
}
