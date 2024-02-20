using BlogApp.Business.Abstracts;
using BlogApp.Business.Concretes;
using BlogApp.Business.FluentValidations;
using BlogApp.Business.Helpers.Images;
using BlogApp.Entities.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;

namespace BlogApp.Business.Extensions
{
    public static class BusinessExtension
    {
        public static IServiceCollection LoadBusinessExtension(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddScoped<IAppUserService, AppUserManager>();
            services.AddScoped<IImageService, ImageManager>();
            services.AddScoped<IArticleService, ArticleManager>();
            services.AddScoped<ICategoryService, CategoryManager>();
            services.AddScoped<IRoleService, RoleManager>();
            services.AddScoped<IImageHelper, ImageHelper>();
            services.AddScoped<IDashboardService, DashboardManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();



            services.AddAutoMapper(assembly);

            services.AddControllersWithViews().AddFluentValidation(opt =>
            {
                //her entity için tanımlama yapmaya gerek yok çünkü ArticleValidator'un bulunduğu Assembly içersinde ki AbstractValidator'dan türeyenlerin hepsini buraya ekler. 
                opt.RegisterValidatorsFromAssemblyContaining<ArticleValidator>();
                opt.DisableDataAnnotationsValidation = true;
                opt.ValidatorOptions.LanguageManager.Culture = new CultureInfo("tr");
            });

            return services;
        }
    }
}
