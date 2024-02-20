using BlogApp.Core.Entities;
using BlogApp.Entities.Entities;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Entities.Entities
{
    public class AppUser : IdentityUser<Guid>,IEntityBase
    {
        public AppUser()
        {
                
        }
        public AppUser(string username, string firstName, string lastName, string email, string phoneNumber)
        {
            UserName = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;

        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid ImageId { get; set; }
        public Image Image { get; set; }

        public ICollection<Article> Articles { get; set; }
    }

    
}
