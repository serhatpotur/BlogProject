using BlogApp.Entities.DTOs.Images;
using BlogApp.Entities.Entities;
using BlogApp.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Business.Abstracts
{
    public interface IImageService
    {
        Task<ImageUploadedDto> UploadAsync(IFormFile file, string folderName);
        Task<Image> AddImageAsync(IFormFile file, string userName, ImageType imageType, string folderName = null);
        void Delete(string imageName);
    }
}
