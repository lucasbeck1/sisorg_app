using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace api.Services
{
    public interface IFilesService
    {
        ServiceResult<string> UploadFile(IFormFile file);
        ServiceResult<string> ReadFile(IFormFile file);
    }

    public class FileService : IFilesService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHost)
        {
            _webHostEnvironment = webHost;
        }

        public ServiceResult<string> ReadFile(IFormFile file)
        {
            try
            {
                if (file.Length == 0)
                    return ServiceResult<string>.Fail("File is empty.");

                // Allowed extensions
                var validExtensions = new[] { ".txt" };

                // File extension
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            
                // File extension validation
                if (!validExtensions.Contains(ext))
                {
                    return ServiceResult<string>.Fail("Invalid file extension!");
                }

                // Read file
                string fileContent;
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    fileContent = reader.ReadToEnd();
                }

                return ServiceResult<string>.Success(fileContent);
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail(ex.Message);
            }
        }

        public ServiceResult<string> UploadFile(IFormFile file)
        {
            try
            {
                if (file.Length == 0)
                    return ServiceResult<string>.Fail("File is empty.");

                // Allowed extensions
                var validExtensions = new[] { ".txt" };

                // File extension
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

                // File extension validation
                if (!validExtensions.Contains(ext))
                {
                    return ServiceResult<string>.Fail("Invalid file extension!");
                }

                // Folder to upload
                var path = Path.Combine(_webHostEnvironment.ContentRootPath, "Files");

                // If the folder doesn´t exist, create
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Complete path
                string fullPath = Path.Combine(path, file.FileName);
                
                // Read and copy to server
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return ServiceResult<string>.Success(fullPath);
            }
            catch (Exception ex)
            {
                return ServiceResult<string>.Fail(ex.Message);
            }
        }
    }
}
