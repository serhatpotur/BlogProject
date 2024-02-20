using BlogApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.DataAccess.Mappings
{
    public class ImageMap : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasData(new Image
            {
                Id = Guid.Parse("10661896-4D45-4104-BDAE-013454E227BB"),
                FileType = "jpg",
                FileName = "images/test",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                isDeleted = false,
                

            });
        }
    }
}
