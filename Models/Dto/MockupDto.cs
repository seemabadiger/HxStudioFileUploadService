namespace HxStudioFileUploadService.Models.Dto
{
    public class MockupDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
       // public Guid Domain { get; set; }
        //public Guid Subdomain { get; set; }
        public bool IsLiked { get; set; }
        public string FilePath { get; set; }
        public List<string> Tags { get; set; }
        public string Domainname { get; set; }
        public string Subdomainname { get; set; }
    }
}
