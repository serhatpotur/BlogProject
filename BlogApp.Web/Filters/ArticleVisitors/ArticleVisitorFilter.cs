using BlogApp.DataAccess.UnitOfWorks.Abstracts;
using BlogApp.Entities.Entities;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApp.Web.Filters.ArticleVisitors
{
    public class ArticleVisitorFilter : IAsyncActionFilter
    {
        private readonly IUnitOfWork _unitOfWork;
        public bool Disabled { get; set; }

        public ArticleVisitorFilter(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //if (Disabled) return next();
            var visitors = _unitOfWork.GetRepository<Visitor>().GetAllAsync().Result;

            var getIpAdress = context.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            string getUserAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();

            Visitor visitor = new()
            {
                IpAdress = getIpAdress,
                UserAgent = getUserAgent,
            };

            if (visitors.Any(x => x.IpAdress == visitor.IpAdress))
                return next(); //kullanıcı ıp adresi varsa devam et

            else
            {
                _unitOfWork.GetRepository<Visitor>().AddAsync(visitor);
                _unitOfWork.Save();
            }

            return next();

        }
    }
}
