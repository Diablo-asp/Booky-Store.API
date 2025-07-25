namespace Booky_Store.API.DTOs.Request
{
    public class CreateAuthorRequest
    {
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }
        public IFormFile ImageUrl { get; set; } = null!;
    }
}
