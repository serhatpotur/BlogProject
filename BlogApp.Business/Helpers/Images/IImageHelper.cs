using BlogApp.Entities.DTOs.Images;
using BlogApp.Entities.Enums;
using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Helpers.Images
{
    public interface IImageHelper
    {
        Task<ImageUploadedDto> UploadAsync(string name,IFormFile file,ImageType imageType, string folderName=null);
        void Delete(string imageName);
    }
}
