namespace BlogApp.Entities.DTOs.Users
{
    public class AppUserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
