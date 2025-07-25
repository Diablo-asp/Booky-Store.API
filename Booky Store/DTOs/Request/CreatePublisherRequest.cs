namespace Booky_Store.API.DTOs.Request
{
    public class CreatePublisherRequest
    {
        [Required(ErrorMessage = "Publisher Name Is Required")]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        
        public IFormFile LogoUrl { get; set; }
    }
}
