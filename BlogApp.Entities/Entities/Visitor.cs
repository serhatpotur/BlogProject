using BlogApp.Core.Entities;

namespace BlogApp.Entities.Entities
{
    public class Visitor : IEntityBase
    {
        public Visitor()
        {

        }
        public Visitor(string ipAdress, string userAgent)
        {
            IpAdress = ipAdress;
            UserAgent = userAgent;
        }
        public int Id { get; set; }
        public string IpAdress { get; set; }
        public string UserAgent { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public ICollection<ArticleVisitor> ArticleVisitors { get; set; }

    }
}
