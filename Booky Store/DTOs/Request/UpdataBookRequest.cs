

namespace Booky_Store.API.DTOs.Request
{
    public class UpdataBookRequest
    {
        [Required(ErrorMessage = "Title Is Required")]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        [Range(0, 1000000)]
        public double Price { get; set; }
        public string ISBN { get; set; } = null!;
        public DateOnly? PublishDate { get; set; }
        public int CategoryId { get; set; }
        public int PublisherId { get; set; }

        public List<int> AuthorIds { get; set; } = new();
        public IFormFile? CoverImageUrl { get; set; }
    }
}
