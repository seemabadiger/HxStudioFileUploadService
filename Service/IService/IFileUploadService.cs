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
        Task<FileUploadResponseDto> UploadFilesAsync(Guid userId, FileUploadRequestDto fileUploadDto);
        Task<bool> LikeMockupAsync(Guid userId, int mockupId, bool isLiked);
        Task<IEnumerable<MockupGroupDto>> GetMockupsByUserAsync(Guid userId);
        Task<Domain> AddDomainAsync(DomainDto domain);
        Task<Subdomain> AddSubdomainAsync(SubdomainDto subdomain);
        Task<List<Subdomain>> GetSubdomainsAsync(int domainId);
        Task<List<Domain>> GetDomainsAsync();
        Task<IEnumerable<MockupGroupDto>> GetRecentMockupsByUserAsync(Guid userId, int days = 1);
        Task<IEnumerable<MockupGroupDto>> GetMockupsByUserByNameAsync(Guid userId);
        Task<IEnumerable<MockupGroupDto>> SearchMockupsAsync(Guid userId, string searchTerm);
    
        Task<IEnumerable<MockupGroupDto>> SearchMockupsByDomainAsync(Guid userId, string domainName);
        Task<FileUploadResponseDto> UpdateTemplateAsync(int id, FileUploadRequestDto mockupUpdateDto, Guid userId);
        Task<FileUploadResponseDto> DeleteTemplateAsync(int id);


    }
}
