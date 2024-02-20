using BlogApp.Entities.Entities;
using FluentValidation;

namespace BlogApp.Business.FluentValidations
{
    public class ArticleValidator:AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(x=>x.Title).NotEmpty().NotEmpty().MinimumLength(5).MaximumLength(50).WithName("Başlık");
            RuleFor(x=>x.Content).NotEmpty().NotEmpty().MinimumLength(5).WithName("İçerik");
        }
    }
}
