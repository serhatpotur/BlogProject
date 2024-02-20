using BlogApp.Entities.DTOs.Images;
using BlogApp.Entities.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Helpers.Images
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _wwwroot;
        private const string imagesFolderName = "images";
        private const string articleImagesFolderName = "article-images";
        private const string userImagesFolderName = "user-images";

        public ImageHelper(IWebHostEnvironment environment)
        {
            _environment = environment;
            _wwwroot = _environment.WebRootPath; //wwwroot pathi
        }

        public void Delete(string imageName)
        {
            var fileToDelete = Path.Combine($"{_wwwroot}/{imagesFolderName}/{imageName}");
            if (File.Exists(fileToDelete))
                File.Delete(fileToDelete);
        }

        public async Task<ImageUploadedDto> UploadAsync(string name, IFormFile file, ImageType imageType, string folderName = null)
        {
            folderName ??= imageType == ImageType.User ? userImagesFolderName : articleImagesFolderName;
            string folderPath = $"{_wwwroot}/{imagesFolderName}/{folderName}";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string oldFileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName);

            string newFileName = $"{Guid.NewGuid()}{fileExtension}";
            //string newFileName = Path.Combine(Guid.NewGuid().ToString(), fileExtension);

            var path = Path.Combine(folderPath, newFileName);

            await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, false);
            await file.CopyToAsync(stream);
            await stream.FlushAsync();

            return new ImageUploadedDto
            {
                FullName = $"{folderName}/{newFileName}"
            };
        }

        private string ReplaceInvalidChars(string fileName)
        {
            return fileName.Replace("İ", "I")
                 .Replace("ı", "i")
                 .Replace("Ğ", "G")
                 .Replace("ğ", "g")
                 .Replace("Ü", "U")
                 .Replace("ü", "u")
                 .Replace("ş", "s")
                 .Replace("Ş", "S")
                 .Replace("Ö", "O")
                 .Replace("ö", "o")
                 .Replace("Ç", "C")
                 .Replace("ç", "c")
                 .Replace("é", "")
                 .Replace("!", "")
                 .Replace("'", "")
                 .Replace("^", "")
                 .Replace("+", "")
                 .Replace("%", "")
                 .Replace("/", "")
                 .Replace("(", "")
                 .Replace(")", "")
                 .Replace("=", "")
                 .Replace("?", "")
                 .Replace("_", "")
                 .Replace("*", "")
                 .Replace("æ", "")
                 .Replace("ß", "")
                 .Replace("@", "")
                 .Replace("€", "")
                 .Replace("<", "")
                 .Replace(">", "")
                 .Replace("#", "")
                 .Replace("$", "")
                 .Replace("½", "")
                 .Replace("{", "")
                 .Replace("[", "")
                 .Replace("]", "")
                 .Replace("}", "")
                 .Replace(@"\", "")
                 .Replace("|", "")
                 .Replace("~", "")
                 .Replace("¨", "")
                 .Replace(",", "")
                 .Replace(";", "")
                 .Replace("`", "")
                 .Replace(".", "")
                 .Replace(":", "")
                 .Replace(" ", "");
        }
    }
}
