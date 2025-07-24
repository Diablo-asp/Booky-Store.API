using System.ComponentModel.DataAnnotations;

namespace Booky_Store.API.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Publisher Name Is Required")]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool? IsBestSeller { get; set; } = false;
        public int SellsCount { get; set; }
        public string? LogoUrl { get; set; }
    }
}
