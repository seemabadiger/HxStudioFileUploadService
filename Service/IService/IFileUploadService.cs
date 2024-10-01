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
        Task<FileUploadResponseDto> UploadFilesAsync(int userId, FileUploadRequestDto fileUploadDto);
        Task<bool> LikeMockupAsync(int userId, int mockupId, bool isLiked);
        Task<IEnumerable<MockupGroupDto>> GetMockupsByUserAsync(int userId);
        Task<Domain> AddDomainAsync(DomainDto domain);
        Task<Subdomain> AddSubdomainAsync(SubdomainDto subdomain);
        Task<List<Subdomain>> GetSubdomainsAsync(int domainId);
        Task<List<Domain>> GetDomainsAsync();
        Task<IEnumerable<MockupGroupDto>> GetRecentMockupsByUserAsync(int userId, int days = 1);
        Task<IEnumerable<MockupGroupDto>> GetMockupsByUserByNameAsync(int userId);
        Task<IEnumerable<MockupGroupDto>> SearchMockupsAsync(int userId, string searchTerm);
    
        Task<IEnumerable<MockupGroupDto>> SearchMockupsByDomainAsync(int userId, string domainName);
        Task<FileUploadResponseDto> UpdateTemplateAsync(int id, FileUploadRequestDto mockupUpdateDto, int userId);
        Task<FileUploadResponseDto> DeleteTemplateAsync(int id);


    }
}
