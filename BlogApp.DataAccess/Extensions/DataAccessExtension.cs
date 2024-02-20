using BlogApp.DataAccess.Context;
using BlogApp.DataAccess.Repositories.Abstracts;
using BlogApp.DataAccess.Repositories.Concretes;
using BlogApp.DataAccess.UnitOfWorks.Abstracts;
using BlogApp.DataAccess.UnitOfWorks.Concretes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.DataAccess.Extensions
{
    public static class DataAccessExtension
    {
        public static IServiceCollection LoadDataAccessExtension(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("BlogConn"));
            });

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
