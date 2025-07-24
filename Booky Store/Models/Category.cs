using System.ComponentModel.DataAnnotations;

namespace Booky_Store.API.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
        public string? ImgUrl { get; set; }
    }
}
