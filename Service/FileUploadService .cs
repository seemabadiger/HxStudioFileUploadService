using HxStudioFileUploadService.Data;
using HxStudioFileUploadService.Models;
using HxStudioFileUploadService.Models.Dto;
using HxStudioFileUploadService.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure;
using System.Linq;
namespace HxStudioFileUploadService.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<FileUploadService> _logger;
        private readonly UploadSettings _uploadSettings;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobContainer;

        public FileUploadService(AppDbContext db, IOptions<UploadSettings> uploadSettings, ILogger<FileUploadService> logger)
        {
            _db = db;
            _uploadSettings = uploadSettings.Value;
            _logger = logger;

            // Initialize Azure Blob Storage
            _blobServiceClient = new BlobServiceClient(_uploadSettings.AzureStorageConnectionString);
            _blobContainer = _blobServiceClient.GetBlobContainerClient(_uploadSettings.AzureStorageContainerName);
        }

        public async Task<FileUploadResponseDto> UploadFilesAsync(Guid userId, FileUploadRequestDto mockupUploadDto)
        {
            var response = new FileUploadResponseDto();

            var domain = await AddDomainAsync(new DomainDto { Name = mockupUploadDto.DomainName });
            var subdomain = await AddSubdomainAsync(new SubdomainDto { Name = mockupUploadDto.SubdomainName, DomainId = domain.Id, Domain = domain });
            var mockUpType = await GetMockupType(mockupUploadDto.MockupType);
            var mockups = new List<MockupDto>();
            // Check if files are provided
            if (mockupUploadDto.Mockups == null || !mockupUploadDto.Mockups.Any())
            {
                response.Success = false;
                response.Message = "No files selected";
                _logger.LogWarning("File upload attempt with no files selected.");
                return response;
            }
            try
            {
                foreach (var mockup in mockupUploadDto.Mockups)
                {
                    if (mockup.MockupFile.Length > 0)
                    {
                        var filePath = await UploadFileToBlobStorage(mockup.MockupFile);
                        mockups.Add(new MockupDto { FileName = mockup.MockupFile.FileName, FilePath = filePath, Tags = mockup.Tags });
                    }
                }

                var mockupGroupDto = new MockupGroupDto
                {
                    ProjectTitle = mockupUploadDto.ProjectTitle,
                    ProjectDescription = mockupUploadDto.ProjectDescription,
                    DomainId = domain.Id,
                    SubDomainId = subdomain.Id,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    MockupTypeId = mockUpType?.Id ?? 1
                };
                var mockupGroup = await AddMockUpGroup(mockupGroupDto);
                await AddMockUpFiles(mockups, mockupGroup.Id);
                mockupGroupDto.Id = mockupGroup.Id;

                // Prepare response
                response.Success = true;
                response.Message = "Files uploaded successfully";

                _logger.LogInformation("Files uploaded successfully. Details: Mockup Group Id: " + mockupGroup.Id);
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
        public async Task<FileUploadResponseDto> UploadProcessDiagram(ProcessDiagramFileUploadRequestDto processDiagramFileUploadRequestDto)
        {
            var response = new FileUploadResponseDto();
            // Check if the process type exists
            var processType = await _db.ProcessTypes.FirstOrDefaultAsync(m => m.Id == processDiagramFileUploadRequestDto.ProcessTypeId);
            var deliverable = await _db.Deliverables.FirstOrDefaultAsync(m => m.Id == processDiagramFileUploadRequestDto.DeliverableId);
            if (processType == null || deliverable == null)
            {
                response.Success = false;
                response.Message = "Process type/Deliverable type not found";
                return response;
            }

            // Upload the deliverable files
            if (processDiagramFileUploadRequestDto.DeliverableFile != null || processDiagramFileUploadRequestDto.DeliverableLink != null)
            {
                var processDiagram = new ProcessDiagram
                {
                    ProcessTypeId = processDiagramFileUploadRequestDto.ProcessTypeId,
                    DeliverableId = processDiagramFileUploadRequestDto.DeliverableId,
                    DeliverableLink = processDiagramFileUploadRequestDto.DeliverableLink,
                    CreatedBy = processDiagramFileUploadRequestDto.UserId,
                    CreatedDate = DateTime.Now  
                };
                if (processDiagramFileUploadRequestDto.DeliverableFile !=null && processDiagramFileUploadRequestDto.DeliverableFile.Length > 0)
                {
                    // Save file path in database
                    var filePath = await UploadFileToBlobStorage(processDiagramFileUploadRequestDto.DeliverableFile);
                    processDiagram.DeliverableFilePath = filePath;
                    processDiagram.DeliverableFileName = processDiagramFileUploadRequestDto.DeliverableFile.FileName;

                }
                await AddProcessDiagram(processDiagram);
                response.Success = true;
                response.Message = "Process Diagrams and Artifacts uploaded successfully";
            }
            return response;
        }
        public async Task<FileUploadResponseDto> UploadCaseStudy(Guid userId, FileUploadRequestDto mockupUploadDto)
        {
            var response = new FileUploadResponseDto();
            var caseStudy = new CaseStudy();
            var domain = await AddDomainAsync(new DomainDto { Name = mockupUploadDto.DomainName });
            var subdomain = await AddSubdomainAsync(new SubdomainDto { Name = mockupUploadDto.SubdomainName, DomainId = domain.Id, Domain = domain });
            var mockUpType = await GetMockupType(mockupUploadDto.MockupType);
            // Check if files are provided
            if (mockupUploadDto.CaseStudy == null)
            {
                response.Success = false;
                response.Message = "No files selected";
                _logger.LogWarning("File upload attempt with no files selected.");
                return response;
            }
            try
            {

                if (mockupUploadDto.CaseStudy.CaseStudyFile.Length > 0)
                {
                    // Save file path in database
                    var filePath = await UploadFileToBlobStorage(mockupUploadDto.CaseStudy.CaseStudyFile);
                    caseStudy.CaseStudyFileName = mockupUploadDto.CaseStudy.CaseStudyFile.FileName;
                    caseStudy.CaseStudyFilePath = filePath;


                }
                if (mockupUploadDto.CaseStudy.ThumbnailImage.Length > 0)
                {
                    // Save file path in database
                    var filePath = await UploadFileToBlobStorage(mockupUploadDto.CaseStudy.ThumbnailImage);
                    caseStudy.ThumbnailImageName = mockupUploadDto.CaseStudy.ThumbnailImage.FileName;
                    caseStudy.ThumbnailImagePath = filePath;
                }

                var caseStudyDto = new MockupGroupDto
                {
                    ProjectTitle = mockupUploadDto.ProjectTitle,
                    ProjectDescription = mockupUploadDto.ProjectDescription,
                    DomainId = domain.Id,
                    SubDomainId = subdomain.Id,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    MockupTypeId = mockUpType?.Id ?? 2
                };
                var mockupGroup = await AddMockUpGroup(caseStudyDto);

                caseStudy.MockupGroupId = caseStudyDto.Id;
                caseStudy.Tags = mockupUploadDto.CaseStudy.Tags;
                await AddCaseStudy(caseStudy);
                // Prepare response
                response.Success = true;
                response.Message = "Files uploaded successfully";

                _logger.LogInformation("Files uploaded successfully. Details: Mockup Group Id: " + mockupGroup.Id);
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
        public async Task<FileUploadResponseDto> UploadBeforeAfter(Guid userId, FileUploadRequestDto mockupUploadDto)
        {
            var response = new FileUploadResponseDto();
            var beforeAfter = new BeforeAfter();
            var domain = await AddDomainAsync(new DomainDto { Name = mockupUploadDto.DomainName });
            var subdomain = await AddSubdomainAsync(new SubdomainDto { Name = mockupUploadDto.SubdomainName, DomainId = domain.Id, Domain = domain });
            var mockUpType = await GetMockupType(mockupUploadDto.MockupType);
            // Check if files are provided
            if (mockupUploadDto.BeforeAfter == null)
            {
                response.Success = false;
                response.Message = "No files selected";
                _logger.LogWarning("File upload attempt with no files selected.");
                return response;
            }
            try
            {

                if (mockupUploadDto.BeforeAfter.BeforeDesignFile !=null && mockupUploadDto.BeforeAfter.BeforeDesignFile.Length > 0)
                {
                    // Save file path in database
                    beforeAfter.BeforeDesignFileName = mockupUploadDto.BeforeAfter.BeforeDesignFile.FileName;
                    beforeAfter.BeforeDesignFilePath = await UploadFileToBlobStorage(mockupUploadDto.BeforeAfter.BeforeDesignFile);
                    beforeAfter.BeforeTags = mockupUploadDto.BeforeAfter.BeforeTags;
                }
                if (mockupUploadDto.BeforeAfter.AfterDesignFile.Length > 0)
                {
                    // Save file path in database
                    beforeAfter.AfterDesignFileName = mockupUploadDto.BeforeAfter.AfterDesignFile.FileName;
                    beforeAfter.AfterDesignFilePath = await UploadFileToBlobStorage(mockupUploadDto.BeforeAfter.AfterDesignFile);
                    beforeAfter.AfterTags = mockupUploadDto.BeforeAfter.AfterTags;
                }

                var beforeAfterDto = new MockupGroupDto
                {
                    ProjectTitle = mockupUploadDto.ProjectTitle,
                    ProjectDescription = mockupUploadDto.ProjectDescription,
                    DomainId = domain.Id,
                    SubDomainId = subdomain.Id,
                    CreatedBy = userId,
                    CreatedDate = DateTime.Now,
                    MockupTypeId = mockUpType?.Id ?? 4
                };
                var mockupGroup = await AddMockUpGroup(beforeAfterDto);

                beforeAfter.MockupGroupId = beforeAfterDto.Id;

                await AddBeforeAfter(beforeAfter);
                // Prepare response
                response.Success = true;
                response.Message = "Files uploaded successfully";

                _logger.LogInformation("Files uploaded successfully. Details: Mockup Group Id: " + mockupGroup.Id);
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
        public async Task<FileUploadResponseDto> UpdateTemplateAsync(int id, FileUploadRequestDto mockupUpdateDtos, Guid userId)
        {
            var response = new FileUploadResponseDto();
            var mockups = new List<MockupDto>();
            // Find the existing mockup
            var existingMockup = _db.MockupGroups.FirstOrDefault(m => m.Id == id);
            if (existingMockup == null)
            {
                response.Success = false;
                response.Message = "Template not found";
                _logger.LogWarning($"Attempt to update a non-existing template with ID: {id}");
                return response;
            }
            try
            {
                if (mockupUpdateDtos.MockupFiles != null && mockupUpdateDtos.MockupFiles.Any())
                {
                    foreach (var file in mockupUpdateDtos.MockupFiles)
                    {
                        if (file.Length > 0)
                        {
                            // Generate unique file name
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            // Check if container exists, if not exists then create it
                            if (!_blobServiceClient.GetBlobContainerClient(_uploadSettings.AzureStorageContainerName).Exists())
                            {
                                await _blobServiceClient.CreateBlobContainerAsync(_uploadSettings.AzureStorageContainerName);
                            }
                            // Upload file to Azure Blob Storage
                            var blobClient = _blobContainer.GetBlobClient(fileName);
                            using (var fileStream = file.OpenReadStream())
                            {
                                await blobClient.UploadAsync(fileStream, true);
                            }

                            // Save file path in database
                            var filePath = blobClient.Uri.ToString();
                            mockups.Add(new MockupDto { FileName = file.FileName, FilePath = filePath, MockupGroupId = existingMockup.Id });
                        }
                    }
                }
                var domain = await AddDomainAsync(new DomainDto { Name = mockupUpdateDtos.DomainName });
                var subdomain = await AddSubdomainAsync(new SubdomainDto { Name = mockupUpdateDtos.SubdomainName });
                var mockupType = _db.MockupTypes.FirstOrDefault(m => m.Name == mockupUpdateDtos.MockupType);
                existingMockup.ProjectTitle = mockupUpdateDtos.ProjectTitle;
                existingMockup.ProjectDescription = mockupUpdateDtos.ProjectDescription;
                existingMockup.DomainId = domain.Id;
                existingMockup.SubDomainId = subdomain.Id;
                existingMockup.MockupTypeId = mockupType.Id;
                existingMockup.ModifiedBy = userId;
                existingMockup.ModifiedDate = DateTime.Now;
                if (mockups.Any()) await AddMockUpFiles(mockups, existingMockup.Id);

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
        public async Task<FileUploadResponseDto> DeleteTemplateAsync(int id)
        {
            var response = new FileUploadResponseDto();

            try
            {
                // Find the existing mockup
                var mockupGroups = await _db.MockupGroups.FirstOrDefaultAsync(m => m.Id == id);
                var mockups = await _db.Mockups.Where(m => m.MockupGroupId == id).ToListAsync();
                if (mockupGroups == null)
                {
                    response.Success = false;
                    response.Message = "Template not found";
                    _logger.LogWarning($"Attempt to delete a non-existing template with ID: {id}");
                    return response;
                }
                if (mockups != null && mockups.Any())
                {
                    foreach (var mockup in mockups)
                    {
                        // Delete the file from disk
                        if (System.IO.File.Exists(mockup.FilePath))
                        {
                            System.IO.File.Delete(mockup.FilePath);
                        }

                        // Delete the file from Azure Blob Storage
                        var blobUri = new Uri(mockup.FilePath);
                        var blobName = Path.GetFileName(blobUri.LocalPath);
                        var blobClient = _blobContainer.GetBlobClient(blobName);
                        await blobClient.DeleteIfExistsAsync();

                        // Remove from database
                        _db.Mockups.Remove(mockup);
                    }
                }
                _db.MockupGroups.Remove(mockupGroups);
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
        public async Task<bool> LikeMockupAsync(Guid userId, int mockupGroupId, bool isLiked)
        {
            var userMockupLike = await _db.Likes
                .FirstOrDefaultAsync(uml => uml.UserId == userId && uml.MockupGroupId == mockupGroupId);

            if (userMockupLike == null)
            {
                userMockupLike = new Like
                {
                    UserId = userId,
                    MockupGroupId = mockupGroupId,
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
        public async Task<IEnumerable<MockupGroupDto>> GetMockupsByUserAsync(Guid userId)
        {
            return await _db.MockupGroups
                .Include(x => x.Tags)
                .Include(x => x.Domain)
                .Include(x => x.SubDomain)
                .Include(x => x.Mockups)
                .Include(x => x.Like)
                .Include(x => x.MockupType)
                .Include(x => x.CaseStudy)
                .Include(x => x.BeforeAfter)
                .Select(uml => new MockupGroupDto
                {
                    Id = uml.Id,
                    ProjectTitle = uml.ProjectTitle,
                    ProjectDescription = uml.ProjectDescription,
                    DomainId = uml.DomainId,
                    Domain = new DomainDto { Id = uml.Domain.Id, Name = uml.Domain.Name },
                    SubDomainId = uml.SubDomainId,
                    Subdomain = new SubdomainDto { Id = uml.SubDomainId, Name = uml.SubDomain.Name },
                    Mockups = uml.Mockups.Select(mps => new MockupDto
                    {
                        FileName = mps.FileName,
                        FilePath = mps.FilePath,
                        MockupGroupId = mps.MockupGroupId,
                        Tags = mps.Tags

                    }).ToList(),
                    Tags = uml.Tags,
                    Like = uml.Like,
                    MockupTypeId = uml.MockupTypeId,
                    MockupType = new MockupTypeDto { Id = uml.MockupType.Id, Name = uml.MockupType.Name },
                    IsFavorite = uml.Like != null ? uml.Like.IsLiked : false,
                    CaseStudy = uml.CaseStudy,
                    BeforeAfter = uml.BeforeAfter,
                    CreatedBy = uml.CreatedBy,
                    CreatedDate = uml.CreatedDate,
                    ModifiedBy = uml.ModifiedBy,
                    ModifiedDate = uml.ModifiedDate
                })
                .ToListAsync();
        }
        public async Task<Domain> AddDomainAsync(DomainDto domainDto)
        {
            var domain = new Domain
            {
                Name = domainDto.Name
            };
            if (_db.Domain.Any(x => x.Name == domainDto.Name))
                return await _db.Domain.FirstOrDefaultAsync(x => x.Name == domainDto.Name);
            else
            {
                _db.Domain.Add(domain);
                await _db.SaveChangesAsync();
            }
            return domain;
        }
        public async Task<List<Subdomain>> GetSubdomainsAsync(int domainId)
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
                Name = subdomainDto.Name,
                DomainId = subdomainDto.DomainId
            };
            if (_db.Subdomain.Any(x => x.Name == subdomainDto.Name && x.DomainId == subdomainDto.DomainId))
                return await _db.Subdomain.FirstOrDefaultAsync(x => x.Name == subdomainDto.Name && x.DomainId == subdomainDto.DomainId);
            else
            {
                _db.Subdomain.Add(subDomain);
                await _db.SaveChangesAsync();
            }
            return subDomain;
        }
        public async Task<MockupGroup> AddMockUpGroup(MockupGroupDto mockupGroupDto)
        {

            var mockupGroup = new MockupGroup
            {
                ProjectTitle = mockupGroupDto.ProjectTitle,
                ProjectDescription = mockupGroupDto.ProjectDescription,
                DomainId = mockupGroupDto.DomainId,
                SubDomainId = mockupGroupDto.SubDomainId,
                MockupTypeId = mockupGroupDto.MockupTypeId,
                CreatedBy = mockupGroupDto.CreatedBy,
                CreatedDate = mockupGroupDto.CreatedDate
            };

            _db.MockupGroups.Add(mockupGroup);
            await _db.SaveChangesAsync();
            return mockupGroup;
        }
        public async Task<List<Tag>> AddTagsToMockupGroup(MockupGroupDto mockupGroupDto)
        {

            mockupGroupDto.Tags.ForEach(x => x.MockupGroupId = mockupGroupDto.Id);
            _db.Tags.AddRange(mockupGroupDto.Tags);
            await _db.SaveChangesAsync();
            return mockupGroupDto.Tags;
        }
        public async Task<List<Mockup>> AddMockUpFiles(List<MockupDto> mockupDto, int mockUpGroupId)
        {
            var existingMockups = await _db.Mockups.Where(x => x.MockupGroupId == mockUpGroupId).ToListAsync();
            if (existingMockups != null && existingMockups.Any())
            {
                _db.Mockups.RemoveRange(existingMockups);
            }
            var mockups = mockupDto.Select(dto => new Mockup
            {
                FileName = dto.FileName,
                FilePath = dto.FilePath,
                MockupGroupId = mockUpGroupId,
                Tags = dto.Tags
            }).ToList();

            _db.Mockups.AddRange(mockups);
            await _db.SaveChangesAsync();

            return mockups;
        }
        public async Task<IEnumerable<MockupGroupDto>> GetRecentMockupsByUserAsync(Guid userId, int days = 1)
        {
            var recentDate = DateTime.Now.AddDays(-days);

            return await _db.MockupGroups
                 .Include(x => x.Tags)
                .Include(x => x.Domain)
                .Include(x => x.SubDomain)
                .Include(x => x.Mockups)
                .Include(x => x.Like)
                 .Include(x => x.CaseStudy)
                .Include(x => x.BeforeAfter)
                 //.Where(m => m.CreatedBy == userId && m.CreatedDate >= recentDate)
                 .Where(m => m.CreatedDate >= recentDate)
                .Select(m => new MockupGroupDto
                {

                    Id = m.Id,
                    ProjectTitle = m.ProjectTitle,
                    ProjectDescription = m.ProjectDescription,
                    DomainId = m.DomainId,
                    Domain = new DomainDto { Id = m.Domain.Id, Name = m.Domain.Name },
                    SubDomainId = m.SubDomainId,
                    Subdomain = new SubdomainDto { Id = m.SubDomainId, Name = m.SubDomain.Name },
                    Mockups = m.Mockups.Select(mps => new MockupDto
                    {
                        FileName = mps.FileName,
                        FilePath = mps.FilePath,
                        MockupGroupId = mps.MockupGroupId,
                        Tags=mps.Tags
                    }).ToList(),
                    Tags = m.Tags,
                    MockupTypeId = m.MockupTypeId,
                    CaseStudy = m.CaseStudy,
                    BeforeAfter = m.BeforeAfter,
                    CreatedBy = m.CreatedBy,
                    CreatedDate = m.CreatedDate,
                    ModifiedBy = m.ModifiedBy,
                    ModifiedDate = m.ModifiedDate,
                    Like = m.Like
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<MockupGroupDto>> GetMockupsByUserByNameAsync(Guid userId)
        {
            return await _db.MockupGroups
                 .Include(x => x.Tags)
                .Include(x => x.Domain)
                .Include(x => x.SubDomain)
                .Include(x => x.Mockups)
                .Include(x => x.Like)
                 .Include(x => x.CaseStudy)
                .Include(x => x.BeforeAfter)
                //.Where(m => m.CreatedBy == userId)
                .OrderBy(m => m.ProjectTitle) // Order alphabetically by name
                .Select(m => new MockupGroupDto
                {

                    Id = m.Id,
                    ProjectTitle = m.ProjectTitle,
                    ProjectDescription = m.ProjectDescription,
                    DomainId = m.DomainId,
                    Domain = new DomainDto { Id = m.Domain.Id, Name = m.Domain.Name },
                    SubDomainId = m.SubDomainId,
                    Subdomain = new SubdomainDto { Id = m.SubDomainId, Name = m.SubDomain.Name },
                    Mockups = m.Mockups.Select(mps => new MockupDto
                    {
                        FileName = mps.FileName,
                        FilePath = mps.FilePath,
                        MockupGroupId = mps.MockupGroupId,
                        Tags=mps.Tags
                    }).ToList(),
                    Tags = m.Tags,
                    MockupTypeId = m.MockupTypeId,
                    CaseStudy = m.CaseStudy,
                    BeforeAfter = m.BeforeAfter,
                    CreatedBy = m.CreatedBy,
                    CreatedDate = m.CreatedDate,
                    ModifiedBy = m.ModifiedBy,
                    ModifiedDate = m.ModifiedDate,
                    Like = m.Like
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<MockupGroupDto>> SearchMockupsAsync(Guid userId, string searchTerm)
        {
            // Sanitize and handle search term for security reasons
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Enumerable.Empty<MockupGroupDto>();
            }

            // Convert search term to lower case for case-insensitive search
            var lowerSearchTerm = searchTerm.ToLower();

            return await _db.MockupGroups
                 .Include(x => x.Tags)
                .Include(x => x.Domain)
                .Include(x => x.SubDomain)
                .Include(x => x.Mockups)
                .Include(x => x.Like)
                 .Include(x => x.CaseStudy)
                .Include(x => x.BeforeAfter)
                .Where(m => m.CreatedBy == userId && (
                    m.ProjectTitle.ToLower().Contains(lowerSearchTerm) ||
                      (m.Tags != null && m.Tags.Any(tag => tag.Name.ToLower().Contains(lowerSearchTerm))) ||
                    m.Domain.Name.ToLower().Contains(lowerSearchTerm) ||
                    m.SubDomain.Name.ToLower().Contains(lowerSearchTerm)
                ))
                .Select(m => new MockupGroupDto
                {
                    Id = m.Id,
                    ProjectTitle = m.ProjectTitle,
                    ProjectDescription = m.ProjectDescription,
                    DomainId = m.DomainId,
                    Domain = new DomainDto { Id = m.Domain.Id, Name = m.Domain.Name },
                    SubDomainId = m.SubDomainId,
                    Subdomain = new SubdomainDto { Id = m.SubDomainId, Name = m.SubDomain.Name },
                    Mockups = m.Mockups.Select(mps => new MockupDto
                    {
                        FileName = mps.FileName,
                        FilePath = mps.FilePath,
                        MockupGroupId = mps.MockupGroupId,
                        Tags= mps.Tags
                    }).ToList(),
                    Tags = m.Tags,
                    MockupTypeId = m.MockupTypeId,
                    CaseStudy = m.CaseStudy,
                    BeforeAfter = m.BeforeAfter,
                    CreatedBy = m.CreatedBy,
                    CreatedDate = m.CreatedDate,
                    ModifiedBy = m.ModifiedBy,
                    ModifiedDate = m.ModifiedDate,
                    Like = m.Like
                })
                .ToListAsync();
        }
        public async Task<IEnumerable<MockupGroupDto>> SearchMockupsByDomainAsync(Guid userId, string domainName)
        {
            IQueryable<MockupGroup> query = _db.MockupGroups.Include(x => x.Tags)
                .Include(x => x.Domain)
                .Include(x => x.SubDomain)
                .Include(x => x.Mockups)
                .Include(x => x.Like)
                 .Include(x => x.CaseStudy)
                .Include(x => x.BeforeAfter);
            //.Where(m => m.CreatedBy == userId);

            if (!string.IsNullOrWhiteSpace(domainName) && domainName.ToLower() != "all")
            {
                var lowerDomainName = domainName.ToLower();
                query = query.Where(m => m.Domain.Name.ToLower().Contains(lowerDomainName) || m.Tags.Any(x => x.Name.ToLower().Contains(lowerDomainName)));
            }

            return await query
                .Select(m => new MockupGroupDto
                {
                    Id = m.Id,
                    ProjectTitle = m.ProjectTitle,
                    ProjectDescription = m.ProjectDescription,
                    DomainId = m.DomainId,
                    Domain = new DomainDto { Id = m.Domain.Id, Name = m.Domain.Name },
                    SubDomainId = m.SubDomainId,
                    Subdomain = new SubdomainDto { Id = m.SubDomainId, Name = m.SubDomain.Name },
                    Mockups = m.Mockups.Select(mps => new MockupDto
                    {
                        FileName = mps.FileName,
                        FilePath = mps.FilePath,
                        MockupGroupId = mps.MockupGroupId,
                        Tags= mps.Tags
                    }).ToList(),
                    Tags = m.Tags,
                    Like = m.Like,
                    MockupTypeId = m.MockupTypeId,
                    CaseStudy = m.CaseStudy,
                    BeforeAfter = m.BeforeAfter,
                    CreatedBy = m.CreatedBy,
                    CreatedDate = m.CreatedDate,
                    ModifiedBy = m.ModifiedBy,
                    ModifiedDate = m.ModifiedDate
                })
                .ToListAsync();
        }
        public async Task<MockupGroupDto> GetMockupGroupDetailsAsync(int mockupGroupId)
        {
            return await _db.MockupGroups
                .Include(x => x.Tags)
                .Include(x => x.Domain)
                .Include(x => x.SubDomain)
                .Include(x => x.Mockups)
                .Include(x => x.Like)
                 .Include(x => x.CaseStudy)
                .Include(x => x.BeforeAfter)
                .Where(m => m.Id == mockupGroupId)
                .Select(m => new MockupGroupDto
                {
                    Id = m.Id,
                    ProjectTitle = m.ProjectTitle,
                    ProjectDescription = m.ProjectDescription,
                    DomainId = m.DomainId,
                    Domain = new DomainDto { Id = m.Domain.Id, Name = m.Domain.Name },
                    SubDomainId = m.SubDomainId,
                    Subdomain = new SubdomainDto { Id = m.SubDomainId, Name = m.SubDomain.Name },
                    Mockups = m.Mockups.Select(mps => new MockupDto
                    {
                        FileName = mps.FileName,
                        FilePath = mps.FilePath,
                        MockupGroupId = mps.MockupGroupId,
                        Tags= mps.Tags
                    }).ToList(),
                    Tags = m.Tags,
                    MockupTypeId = m.MockupTypeId,
                    CaseStudy = m.CaseStudy,
                    BeforeAfter = m.BeforeAfter,
                    CreatedBy = m.CreatedBy,
                    CreatedDate = m.CreatedDate,
                    ModifiedBy = m.ModifiedBy,
                    ModifiedDate = m.ModifiedDate,
                    Like = m.Like
                })
                .FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<MockupType>> GetMockupTypesAsync()
        {
            return await _db.MockupTypes.ToListAsync();
        }
        private async Task<string> UploadFileToBlobStorage(IFormFile file)
        {
            // Generate unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            // Check if container exists, if not exists then create it
            if (!_blobServiceClient.GetBlobContainerClient(_uploadSettings.AzureStorageContainerName).Exists())
            {
                await _blobServiceClient.CreateBlobContainerAsync(_uploadSettings.AzureStorageContainerName);
            }
            // Upload file to Azure Blob Storage
            var blobClient = _blobContainer.GetBlobClient(fileName);

            using (var fileStream = file.OpenReadStream())
            {
                BlobHttpHeaders blobHttpHeaders = new BlobHttpHeaders()
                {
                    ContentType = file.ContentType
                };
                await blobClient.UploadAsync(fileStream, new BlobUploadOptions { HttpHeaders = blobHttpHeaders });
            }

            // Save file path in database
            return blobClient.Uri.ToString();
        }
        private async Task<MockupType?> GetMockupType(string mockupType)
        {
            return _db.MockupTypes.FirstOrDefault(m => m.Name == mockupType);
        }
        private async Task AddProcessDiagram(ProcessDiagram processDiagram)
        {
            var existingProcessDiagrams = await _db.ProcessDiagrams.Where(x => x.ProcessTypeId == processDiagram.ProcessTypeId && x.DeliverableId == processDiagram.DeliverableId).FirstOrDefaultAsync();
            
            if (existingProcessDiagrams != null )
            {
                processDiagram.ModifiedBy = processDiagram.CreatedBy;
                processDiagram.ModifiedDate = processDiagram.CreatedDate;
                processDiagram.CreatedBy = existingProcessDiagrams.CreatedBy;
                processDiagram.CreatedDate = existingProcessDiagrams.CreatedDate;
                _db.ProcessDiagrams.Remove(existingProcessDiagrams);
            }
            _db.ProcessDiagrams.Add(processDiagram);
            await _db.SaveChangesAsync();
        }
        private async Task AddCaseStudy(CaseStudy caseStudy)
        {
            var existingCaseStudies = await _db.CaseStudies.Where(x => x.MockupGroupId == caseStudy.MockupGroupId).FirstOrDefaultAsync();
            if (existingCaseStudies != null)
            {
                caseStudy.ModifiedBy = caseStudy.CreatedBy;
                caseStudy.ModifiedDate = caseStudy.CreatedDate;
                caseStudy.CreatedBy = existingCaseStudies.CreatedBy;
                caseStudy.CreatedDate = existingCaseStudies.CreatedDate;
                _db.CaseStudies.Remove(existingCaseStudies);
            }

            _db.CaseStudies.Add(caseStudy);
            await _db.SaveChangesAsync();
        }
        private async Task AddBeforeAfter(BeforeAfter beforeAfter)
        {
            var existingBeforeAfters = await _db.BeforeAfters.Where(x => x.MockupGroupId == beforeAfter.MockupGroupId).FirstOrDefaultAsync();
            if (existingBeforeAfters != null)
            {
                beforeAfter.ModifiedBy = beforeAfter.CreatedBy;
                beforeAfter.ModifiedDate = beforeAfter.CreatedDate;
                beforeAfter.CreatedBy = existingBeforeAfters.CreatedBy;
                beforeAfter.CreatedDate = existingBeforeAfters.CreatedDate;
                _db.BeforeAfters.Remove(existingBeforeAfters);
            }

            _db.BeforeAfters.Add(beforeAfter);
            await _db.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProcessDiagramResponseDto>> GetProcessDiagramsAsync()
        {
            var processDiagrams = await _db.ProcessDiagrams
                .Include(pd => pd.ProcessType)
                .Include(pd => pd.Deliverable)
                .ToListAsync();

            var result = processDiagrams.GroupBy(pd => pd.ProcessTypeId)
                .Select(g => new ProcessDiagramResponseDto
                {
                    ProcessId = g.Key,
                    ProcessName = g.First().ProcessType.ProcessName,
                    Deliverables = g.Select(pd => new DeliverableDto
                    {
                        DeliverableId = pd.DeliverableId,
                        DeliverableName = pd.Deliverable.DeliverableName,
                        DeliverableFileName = pd.DeliverableFileName,
                        DeliverableFilePath = pd.DeliverableFilePath,
                        DeliverableLink = pd.DeliverableLink
                    }).ToList()
                });

            return result;
        }
        public async Task<IEnumerable<ProcessType>> GetProcessTypesAsync()
        {
            return await _db.ProcessTypes.ToListAsync();
        }

        public async Task<IEnumerable<Deliverable>> GetDeliverablesAsync()
        {
            return await _db.Deliverables.Include(x=>x.ProcessType).ToListAsync();
        }

    }
}
