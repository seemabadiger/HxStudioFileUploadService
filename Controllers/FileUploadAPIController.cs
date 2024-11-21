using HxStudioFileUploadService.Services;
using HxStudioFileUploadService.Models.Dto;
using HxStudioFileUploadService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using HxStudioFileUploadService.Service;

namespace HxStudioFileUploadService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadAPIController : ControllerBase
    {

        private readonly IFileUploadService _fileUploadService;

        public FileUploadAPIController(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> UploadFiles(Guid userId, [FromForm] FileUploadRequestDto mockupUploadDto)
        {
            var response = await _fileUploadService.UploadFilesAsync(userId,mockupUploadDto);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            var response = await _fileUploadService.DeleteTemplateAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }


        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateTemplate(int id, [FromForm] FileUploadRequestDto mockupUpdateDto, Guid userId)
        {
            var response = await _fileUploadService.UpdateTemplateAsync(id, mockupUpdateDto, userId);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("{userId}/like/{mockupGroupId}")]
        public async Task<IActionResult> LikeMockup(Guid userId, int mockupGroupId, [FromBody] bool isLiked)
        {
            var result = await _fileUploadService.LikeMockupAsync(userId, mockupGroupId, isLiked);
            if (result)
            {
                return Ok();
            }

            return BadRequest("Error liking the mockup.");
        }
        [HttpGet("{userId}/mockups")]
        public async Task<IActionResult> GetMockupsByUser(Guid userId)
        {
            try
            {
                var mockups = await _fileUploadService.GetMockupsByUserAsync(userId);
                return Ok(mockups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddDomain([FromBody] DomainDto domain)
        {
            if (domain == null)
            {
                return BadRequest("Domain is null.");
            }

            await _fileUploadService.AddDomainAsync(domain);
            return CreatedAtAction(nameof(GetDomains), domain);
        }
        [HttpPost]
        [Route("subdomain")]
        public async Task<IActionResult> AddSubdomain([FromBody] SubdomainDto subdomainDto)
        {
            if (subdomainDto == null)
            {
                return BadRequest("Subdomain is null.");
            }

            var createdSubdomain = await _fileUploadService.AddSubdomainAsync(subdomainDto);
            if (createdSubdomain == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating subdomain.");
            }

            return CreatedAtAction(nameof(GetSubdomains), new { domainId = createdSubdomain.DomainId, id = createdSubdomain.Id }, createdSubdomain);
        }
        [HttpGet("{domainId}")]
        public async Task<IActionResult> GetSubdomains(int domainId)
        {
            var subdomains = await _fileUploadService.GetSubdomainsAsync(domainId);
            return Ok(subdomains);
        }
        [HttpGet]
        public async Task<IActionResult> GetDomains()
        {
            var domains = await _fileUploadService.GetDomainsAsync();
            return Ok(domains);
        }
        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentMockups(Guid userId)
        {
            try
            {
                var mockups = await _fileUploadService.GetRecentMockupsByUserAsync(userId);
                return Ok(mockups);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("alphabetical")]
        public async Task<IActionResult> GetAllMockups(Guid userId)
        {
            try
            {
                var mockups = await _fileUploadService.GetMockupsByUserByNameAsync(userId);
                return Ok(mockups);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMockups(Guid userId, string query)
        {
            var result = await _fileUploadService.SearchMockupsAsync(userId, query);
            return Ok(result);
        }
        [HttpGet("searchByDomain")]
        public async Task<IActionResult> SearchByDomain([FromQuery] Guid userId, [FromQuery] string domainName)
        {
            var mockups = await _fileUploadService.SearchMockupsByDomainAsync(userId, domainName);

            if (mockups == null || !mockups.Any())
            {
                return NotFound("No mockups found for the specified domain.");
            }

            return Ok(mockups);
        }

        [HttpGet("getmockupgroupdetails/{mockupGroupId}")]
        public async Task<IActionResult> GetMockupGroupDetails(int mockupGroupId)
        {
            var mockupGroupDetails = await _fileUploadService.GetMockupGroupDetailsAsync(mockupGroupId);
            if (mockupGroupDetails == null)
            {
                return NotFound("Mockup group not found.");
            }

            return Ok(mockupGroupDetails);
        }
        [HttpGet("mockuptypes")]
        public async Task<IActionResult> GetMockupTypes()
        {
            try
            {
                var mockupTypes = await _fileUploadService.GetMockupTypesAsync();
                return Ok(mockupTypes);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpPost]
        [Route("uploadcasestudy")]
        public async Task<IActionResult> UploadCaseStudy(Guid userId, [FromForm] CaseStudyFileUploadRequestDto mockupUploadDto)
        {
            var response = await _fileUploadService.UploadCaseStudy(userId, mockupUploadDto);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("uploadbeforeafter")]
        public async Task<IActionResult> UploadBeforeAfter(Guid userId, [FromForm] BeforeAfterFileUploadRequestDto mockupUploadDto)
        {
            var response = await _fileUploadService.UploadBeforeAfter(userId, mockupUploadDto);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("uploadprocessdiagram")]
        public async Task<IActionResult> UploadProcessDiagram([FromForm] ProcessDiagramFileUploadRequestDto processDiagramFileUploadRequestDto)
        {

            var response = await _fileUploadService.UploadProcessDiagram(processDiagramFileUploadRequestDto);
            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }
        }
        [HttpGet]
        [Route("getprocessdiagrams")]
        public async Task<IActionResult> GetProcessDiagramsAsync()
        {
            try
            {
                var processDiagrams = await _fileUploadService.GetProcessDiagramsAsync();
                return Ok(processDiagrams);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
        [HttpGet("processtypes")]
        public async Task<IActionResult> GetProcessTypes()
        {
            try
            {
                var processTypes = await _fileUploadService.GetProcessTypesAsync();
                return Ok(processTypes);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("deliverables")]
        public async Task<IActionResult> GetDeliverables()
        {
            try
            {
                var deliverables = await _fileUploadService.GetDeliverablesAsync();
                return Ok(deliverables);
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

    }
}
