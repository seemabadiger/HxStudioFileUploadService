using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HxStudioFileUploadService.Services;
using HxStudioFileUploadService.Models.Dto;
using HxStudioFileUploadService.Data;
using HxStudioFileUploadService.Models;
using HxStudioFileUploadService.Service;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using HxStudioFileUploadService.Models.Dto;
using HxStudioFileUploadService.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace HxStudioFileUploadService.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly AppDbContext _db;
      //  private readonly IWebHostEnvironment _environment;
        private readonly ILogger<FileUploadService> _logger;
        private readonly UploadSettings _uploadSettings;
        public FileUploadService(AppDbContext db, IOptions<UploadSettings> uploadSettings, ILogger<FileUploadService> logger)
        {
            _db = db;
            _uploadSettings = uploadSettings.Value;
            _logger = logger;
        }

        public async Task<FileUploadResponseDto> UploadFilesAsync(Guid userId,FileUploadRequestDto mockupUploadDto, List<IFormFile> files)
        {
            var response = new FileUploadResponseDto();
            var mockups = new List<Mockup>();

            // Check if files are provided
            if (files == null || !files.Any())
            {
                response.Success = false;
                response.Message = "No files selected";
                _logger.LogWarning("File upload attempt with no files selected.");
                return response;
            }

            // Define upload directory
            var uploads = Path.Combine(_uploadSettings.Path, "uploads");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            try
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var filePath = Path.Combine(uploads, file.FileName);

                        // Save file to disk
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }

                        // Create Mockup object
                        var mockup = new Mockup
                        {
                            Id = Guid.NewGuid(),
                            Name = mockupUploadDto.Name,
                            Domainname = mockupUploadDto.Domainname,
                            Subdomainname = mockupUploadDto.Subdomainname,
                            Tags = mockupUploadDto.Tags,
                            FileName = file.FileName,
                            FilePath = filePath,
                            CreatedBy=userId,
                            CreatedDate=DateTime.Now
                        };

                        mockups.Add(mockup);
                        _db.Mockups.Add(mockup);
                    }
                }

                // Save all mockups to database
                await _db.SaveChangesAsync();

                // Prepare response
                response.Success = true;
                response.Message = "Files uploaded successfully";
                //response.UploadedFiles = mockups.Select(m => new MockupResponseDto
                //{
                //    Id = m.Id,
                //    Name = m.Name,
                //    Domain = m.Domain,
                //    Subdomain = m.Subdomain,
                //    Tags = m.Tags,
                //    FileName = m.FileName,
                //    FilePath = m.FilePath
                //}).ToList();

               // _logger.LogInformation("Files uploaded successfully. Details: " + JsonConvert.SerializeObject(response.UploadedFiles));
            }
            catch (Exception ex)
            {
                // Log and handle exception
                _logger.LogError(ex, "An error occurred while uploading files.");
                response.Success = false;
                response.Message = "An error occurred while processing the files.";
            }

            return response;
        }
        public async Task<FileUploadResponseDto> UpdateTemplateAsync(Guid id, FileUploadRequestDto mockupUpdateDto, List<IFormFile> files)
        {
            var response = new FileUploadResponseDto();

            // Find the existing mockup
            var existingMockup = _db.Mockups.FirstOrDefault(m => m.Id == id);
            if (existingMockup == null)
            {
                response.Success = false;
                response.Message = "Template not found";
                _logger.LogWarning($"Attempt to update a non-existing template with ID: {id}");
                return response;
            }

            // Define upload directory
            var uploads = Path.Combine(_uploadSettings.Path, "uploads");

            try
            {
                if (files != null && files.Any())
                {
                    foreach (var file in files)
                    {
                        if (file.Length > 0)
                        {
                            var filePath = Path.Combine(uploads, file.FileName);

                            // Save file to disk
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(fileStream);
                            }

                            // Update the existing mockup object
                            existingMockup.Name = mockupUpdateDto.Name;
                            existingMockup.Domainname = mockupUpdateDto.Domainname;
                            existingMockup.Subdomainname = mockupUpdateDto.Subdomainname;
                            existingMockup.Tags = mockupUpdateDto.Tags;
                            existingMockup.FileName = file.FileName;
                            existingMockup.FilePath = filePath;
                            existingMockup.ModifiedDate = DateTime.Now;
                        }
                    }
                }
                else
                {
                    // If no files are provided, just update other details
                    existingMockup.Name = mockupUpdateDto.Name;
                    existingMockup.Domainname = mockupUpdateDto.Domainname;
                    existingMockup.Subdomainname = mockupUpdateDto.Subdomainname;
                    existingMockup.Tags = mockupUpdateDto.Tags;
                    existingMockup.ModifiedDate = DateTime.Now;
                }

                // Save changes to database
                await _db.SaveChangesAsync();

                // Prepare response
                response.Success = true;
                response.Message = "Template updated successfully";
            }
            catch (Exception ex)
            {
                // Log and handle exception
                _logger.LogError(ex, "An error occurred while updating the template.");
                response.Success = false;
                response.Message = "An error occurred while updating the template.";
            }

            return response;
        }
        public async Task<FileUploadResponseDto> DeleteTemplateAsync(Guid id)
        {
            var response = new FileUploadResponseDto();

            try
            {
                // Find the existing mockup
                var mockup = _db.Mockups.FirstOrDefault(m => m.Id == id);
                if (mockup == null)
                {
                    response.Success = false;
                    response.Message = "Template not found";
                    _logger.LogWarning($"Attempt to delete a non-existing template with ID: {id}");
                    return response;
                }

                // Delete the file from disk
                if (System.IO.File.Exists(mockup.FilePath))
                {
                    System.IO.File.Delete(mockup.FilePath);
                }

                // Remove from database
                _db.Mockups.Remove(mockup);
                await _db.SaveChangesAsync();

                // Prepare response
                response.Success = true;
                response.Message = "Template deleted successfully";
            }
            catch (Exception ex)
            {
                // Log and handle exception
                _logger.LogError(ex, "An error occurred while deleting the template.");
                response.Success = false;
                response.Message = "An error occurred while deleting the template.";
            }

            return response;
        }

        public async Task<bool> LikeMockupAsync(Guid userId, Guid mockupId, bool isLiked)
        {
            var userMockupLike = await _db.Likes
                .FirstOrDefaultAsync(uml => uml.UserId == userId && uml.MockupId == mockupId);

            if (userMockupLike == null)
            {
                userMockupLike = new Like
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    MockupId = mockupId,
                    IsLiked = isLiked
                };

                _db.Likes.Add(userMockupLike);
            }
            else
            {
                 userMockupLike.IsLiked = isLiked;
            }

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MockupDto>> GetMockupsByUserAsync(Guid userId)
        {
            return await _db.Mockups
                .Select(uml => new MockupDto
                {
                    Id = uml.Id,
                    Name = uml.Name,
                    Domainname = uml.Domainname,
                   Subdomainname = uml.Subdomainname,
                    FilePath = uml.FilePath.Replace("C:\\Uploads\\uploads\\", "http://localhost:8080/"),
                    //FilePath =uml.FilePath,
                    Tags=uml.Tags
                   

                })
                .ToListAsync();
        }
        public async Task<Domain> AddDomainAsync(DomainDto domainDto)
        {
          var  domain = new Domain
            {
                Id = Guid.NewGuid(),
               Name= domainDto.Name
            };
            _db.Domain.Add(domain);
            await _db.SaveChangesAsync();
            return domain;
        }
       
        public async Task<List<Subdomain>> GetSubdomainsAsync(Guid domainId)
        {
            return await _db.Subdomain.Where(s => s.DomainId == domainId).ToListAsync();
        }
        public async Task<List<Domain>> GetDomainsAsync()
        {
            return await _db.Domain.ToListAsync();
        }

        public async Task<Subdomain> AddSubdomainAsync(SubdomainDto subdomainDto)
        {
           
            var subDomain = new Subdomain
            {
                Id = Guid.NewGuid(),
                Name = subdomainDto.Name,
                DomainId=subdomainDto.DomainId
            };
            _db.Subdomain.Add(subDomain);
            await _db.SaveChangesAsync();
            return subDomain;
        }
        public async Task<IEnumerable<MockupDto>> GetRecentMockupsByUserAsync(Guid userId, int days = 1)
        {
            var recentDate = DateTime.Now.AddDays(-days);

            return await _db.Mockups
                .Where(m => m.CreatedBy == userId && m.CreatedDate >= recentDate)
                .Select(m => new MockupDto
                {

                    Id = m.Id,
                    Name = m.Name,
                    Domainname = m.Domainname,
                    Subdomainname = m.Subdomainname,
                    FilePath = m.FilePath.Replace("C:\\Uploads\\uploads\\", "http://localhost:8080/"),
                    //FilePath =uml.FilePath,
                    Tags = m.Tags
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<MockupDto>> GetMockupsByUserByNameAsync(Guid userId)
        {
            return await _db.Mockups
                .Where(m => m.CreatedBy == userId)
                .OrderBy(m => m.Name) // Order alphabetically by name
                .Select(m => new MockupDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Domainname = m.Domainname,
                    Subdomainname = m.Subdomainname,
                    FilePath = m.FilePath.Replace("C:\\Uploads\\uploads\\", "http://localhost:8080/"),
                    //FilePath =uml.FilePath,
                    Tags = m.Tags
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<MockupDto>> SearchMockupsAsync(Guid userId, string searchTerm)
        {
            // Sanitize and handle search term for security reasons
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Enumerable.Empty<MockupDto>();
            }

            // Convert search term to lower case for case-insensitive search
            var lowerSearchTerm = searchTerm.ToLower();

            return await _db.Mockups
                .Where(m => m.CreatedBy == userId && (
                    m.Name.ToLower().Contains(lowerSearchTerm) ||
                      (m.Tags != null && m.Tags.Any(tag => tag.ToLower().Contains(lowerSearchTerm))) ||
                   // m.FileName.ToLower().Contains(lowerSearchTerm) ||
                    m.Domainname.ToLower().Contains(lowerSearchTerm) ||
                    m.Subdomainname.ToLower().Contains(lowerSearchTerm)
                ))
                .Select(m => new MockupDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Domainname = m.Domainname,
                    Subdomainname = m.Subdomainname,
                    FilePath = m.FilePath.Replace("C:\\Uploads\\uploads\\", "http://localhost:8080/"),
                    Tags = m.Tags
                })
                .ToListAsync();
        }
        //public async Task<IEnumerable<MockupDto>> SearchMockupsByDomainAsync(Guid userId, string domainName)
        //{
        //    if (string.IsNullOrWhiteSpace(domainName))
        //    {
        //        return Enumerable.Empty<MockupDto>();
        //    }

        //    var lowerDomainName = domainName.ToLower();

        //    return await _db.Mockups
        //        .Where(m => m.CreatedBy == userId && m.Domainname.ToLower().Contains(lowerDomainName))
        //        .Select(m => new MockupDto
        //        {
        //            Id = m.Id,
        //            Name = m.Name,
        //            Domainname = m.Domainname,
        //            Subdomainname = m.Subdomainname,
        //            FilePath = m.FilePath.Replace("C:\\Uploads\\uploads\\", "http://localhost:8080/"),
        //            Tags = m.Tags
        //        })
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<MockupDto>> SearchMockupsByDomainAsync(Guid userId, string domainName)
        {
            IQueryable<Mockup> query = _db.Mockups.Where(m => m.CreatedBy == userId);

            if (!string.IsNullOrWhiteSpace(domainName) && domainName.ToLower() != "all")
            {
                var lowerDomainName = domainName.ToLower();
                query = query.Where(m => m.Domainname.ToLower().Contains(lowerDomainName));
            }

            return await query
                .Select(m => new MockupDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Domainname = m.Domainname,
                    Subdomainname = m.Subdomainname,
                    FilePath = m.FilePath.Replace("C:\\Uploads\\uploads\\", "http://localhost:8080/"),
                    Tags = m.Tags
                })
                .ToListAsync();
        }


    }
}
