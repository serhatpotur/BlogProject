using BlogApp.Core.Entities;
using BlogApp.Entities.Enums;

namespace BlogApp.Entities.Entities
{
    public class Image : EntityBase
    {
        public Image()
        {
            Users = new HashSet<AppUser>();
        }
        public Image(string fileName, string fileType, string createdBy)
        {
            FileName = fileName;
            FileType = fileType;
            CreatedBy = createdBy;
        }

        public string FileName { get; set; }
        public string FileType { get; set; }

        public ICollection<Article> Articles { get; set; }
        public ICollection<AppUser> Users { get; set; }

    }
}
