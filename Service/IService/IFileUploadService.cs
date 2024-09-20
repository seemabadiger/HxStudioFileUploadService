using System.Collections.Generic;
using System.Threading.Tasks;
using HxStudioFileUploadService.Models.Dto;
using HxStudioFileUploadService.Models;
using Microsoft.AspNetCore.Mvc;
using HxStudioFileUploadService.Service;


namespace HxStudioFileUploadService.Services
{
    public interface IFileUploadService
    {
        Task<FileUploadResponseDto> UploadFilesAsync(Guid userId, FileUploadRequestDto fileUploadDto, List<IFormFile> files);
        Task<bool> LikeMockupAsync(Guid userId, Guid mockupId, bool isLiked);
        Task<IEnumerable<MockupDto>> GetMockupsByUserAsync(Guid userId);
        Task<Domain> AddDomainAsync(DomainDto domain);
        Task<Subdomain> AddSubdomainAsync(SubdomainDto subdomain);
        Task<List<Subdomain>> GetSubdomainsAsync(Guid domainId);
        Task<List<Domain>> GetDomainsAsync();
        Task<IEnumerable<MockupDto>> GetRecentMockupsByUserAsync(Guid userId, int days = 1);
        Task<IEnumerable<MockupDto>> GetMockupsByUserByNameAsync(Guid userId);
        Task<IEnumerable<MockupDto>> SearchMockupsAsync(Guid userId, string searchTerm);
    
        Task<IEnumerable<MockupDto>> SearchMockupsByDomainAsync(Guid userId, string domainName);
        Task<FileUploadResponseDto> UpdateTemplateAsync(Guid id, FileUploadRequestDto mockupUpdateDto, List<IFormFile> files);
        Task<FileUploadResponseDto> DeleteTemplateAsync(Guid id);


    }
}
