using BlogApp.Entities.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Business.FluentValidations
{
    public class UserValidator:AbstractValidator<AppUser>
    {
        public UserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MinimumLength(3).MaximumLength(30).WithName("Ad");
            RuleFor(x => x.LastName).NotEmpty().MinimumLength(3).MaximumLength(30).WithName("Soyad");
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(3).MaximumLength(30).WithName("Kullanıcı Adı");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithName("Email");
            RuleFor(x => x.PhoneNumber).NotEmpty().MinimumLength(10).MaximumLength(11).WithName("Telefon Numarası");
        }
    }
}
