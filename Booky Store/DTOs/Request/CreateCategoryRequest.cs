namespace Booky_Store.API.DTOs.Request
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
        public IFormFile ImgUrl { get; set; }
    }
}
