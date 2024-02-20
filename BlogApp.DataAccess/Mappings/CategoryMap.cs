using BlogApp.Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlogApp.DataAccess.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(new Category
            {
                Id = Guid.Parse("FE98F519-AE0F-4D1B-8387-F9F0570272A6"),
                Name = "Yazılım",
                CreatedBy = "Admin",
                CreatedDate = DateTime.Now,
                isDeleted = false,

            });
        }
    }
}
