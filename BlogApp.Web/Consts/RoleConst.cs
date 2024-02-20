using BlogApp.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Web.Consts
{
    public static class RoleConst
    {
        public const string Admin = "Admin";
        public const string Mod = "Mod";
        public const string User = "User";
    }
}

