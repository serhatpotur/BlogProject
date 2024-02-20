using BlogApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.DataAccess.Mappings
{
    public class ArticleMap : IEntityTypeConfiguration<Article>
    {
        public void Configure(EntityTypeBuilder<Article> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(150);
            builder.HasData(new Article
            {
                Id = Guid.NewGuid(),
                Title = "AspNet Core ile Identity",
                Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris massa ligula, ultrices id congue at, elementum quis orci. Pellentesque mollis nisl nec tortor scelerisque volutpat. Suspendisse euismod arcu eget mauris ultricies congue. Phasellus ut odio id tellus imperdiet iaculis. Donec volutpat rutrum mauris. Maecenas porta efficitur enim",
                CreatedDate = DateTime.Now,
                isDeleted = false,
                ViewCount = 1,
                CreatedBy = "Admin",
                CategoryId = Guid.Parse("FE98F519-AE0F-4D1B-8387-F9F0570272A6"),
                ImageId = Guid.Parse("10661896-4D45-4104-BDAE-013454E227BB"),
                UserId=Guid.Parse("E516E839-8353-4CD1-9D06-616A350E8565")
            });
        }
    }
}
