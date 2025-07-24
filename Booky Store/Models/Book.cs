using System.ComponentModel.DataAnnotations;
using Stripe;

namespace Booky_Store.API.Models
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title Is Required")]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Review { get; set; }
        public double Rate { get; set; }
        public string ISBN { get; set; } = null!;
        public DateOnly PublishDate { get; set; }
        public string? CoverImageUrl { get; set; }


        public Category Category { get; set; }
        public int CategoryId { get; set; }

        public Publisher Publisher { get; set; }
        public int PublisherId { get; set; }

    }
}
