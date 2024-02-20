using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApp.Entities.Entities;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Entities.DTOs.Users
{
    public class AppUserProfileDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IFormFile? FormFile { get; set; }
        public string? ImageId { get; set; }

        public Image? Image { get; set; }


    }
}
