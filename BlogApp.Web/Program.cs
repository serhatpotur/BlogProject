using BlogApp.DataAccess.Context;
using BlogApp.DataAccess.Extensions;
using BlogApp.Business.Extensions;
using Microsoft.EntityFrameworkCore;
using BlogApp.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using NToastNotify;
using BlogApp.Business.Describers;
using BlogApp.Web.Filters.ArticleVisitors;

var builder = WebApplication.CreateBuilder(args);
builder.Services.LoadDataAccessExtension(builder.Configuration);
builder.Services.LoadBusinessExtension();
builder.Services.AddSession();

builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
    // TODO : Test bitince düzelt
    opt.Password.RequireNonAlphanumeric = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.User.RequireUniqueEmail = true;
    opt.User.RequireUniqueEmail = true;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    opt.Lockout.MaxFailedAccessAttempts = 3;
}).AddRoleManager<RoleManager<AppRole>>().
    AddErrorDescriber<CustomIdentityErrorDescriber>().
    AddEntityFrameworkStores<AppDbContext>().
    AddDefaultTokenProviders();


builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = new PathString("/Admin/Auth/Login");
    config.LogoutPath = new PathString("/Admin/Auth/Logout");
    config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied");

    config.Cookie = new CookieBuilder
    {
        Name = "BlogApp",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy = CookieSecurePolicy.SameAsRequest //https ve http tarafýnda istek alýr. TODO : Canlýda Always yap
    };
    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromDays(30);
});
// Add services to the container.
builder.Services.AddControllersWithViews(opt =>
{
    opt.Filters.Add<ArticleVisitorFilter>();
}).AddNToastNotifyToastr(new ToastrOptions
{
    PositionClass = ToastPositions.TopRight,
    TimeOut = 3000,
    ProgressBar = false
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseNToastNotify();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
        name: "Admin",
        areaName: "Admin",
        pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
);

    endpoints.MapDefaultControllerRoute();
});


app.Run();
