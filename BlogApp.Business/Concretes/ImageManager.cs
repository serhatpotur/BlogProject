using BlogApp.Business.Abstracts;
using BlogApp.DataAccess.UnitOfWorks.Abstracts;
using BlogApp.Entities.DTOs.Images;
using BlogApp.Entities.Entities;
using BlogApp.Entities.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Business.Concretes
{
    public class ImageManager : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _wwwroot;
        private const string imagesFolderName = "images";
        private const string articleImagesFolderName = "article-images";
        private const string userImagesFolderName = "user-images";

        public ImageManager(IWebHostEnvironment environment, IUnitOfWork unitOfWork)
        {
            _environment = environment;
            _wwwroot = _environment.WebRootPath; //wwwroot pathi
            _unitOfWork = unitOfWork;
        }

        public void Delete(string imageName)
        {
            var fileToDelete = Path.Combine($"{_wwwroot}/{imagesFolderName}/{imageName}");
            if (File.Exists(fileToDelete))
                File.Delete(fileToDelete);
        }

        public async Task<ImageUploadedDto> UploadAsync(IFormFile file,  string folderName)
        {
            //folderName ??= imageType == ImageType.User ? userImagesFolderName : articleImagesFolderName;
            string folderPath = $"{_wwwroot}/{imagesFolderName}/{folderName}";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            string oldFileName = Path.GetFileNameWithoutExtension(file.FileName);
            string fileExtension = Path.GetExtension(file.FileName);

            string newFileName = $"{Guid.NewGuid()}{fileExtension}";

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

        public async Task<Image> AddImageAsync(IFormFile file, string userName, ImageType imageType, string folderName = null)
        {
            folderName ??= imageType == ImageType.User ? userImagesFolderName : articleImagesFolderName;
            if (file != null)
            {
                var imageUpload = await UploadAsync(file, folderName);
                Image ımage = new(imageUpload.FullName, file.ContentType, userName);


                await _unitOfWork.GetRepository<Image>().AddAsync(ımage);
                await _unitOfWork.SaveAsync();

                return ımage;
            }
            return null;

        }
    }
}
