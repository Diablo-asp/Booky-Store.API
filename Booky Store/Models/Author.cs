using System.ComponentModel.DataAnnotations;

namespace Booky_Store.API.Models
{
    public class Author
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;
        public string? Bio { get; set; }
        public string ImageUrl { get; set; } = null!;        
      
    }
}
