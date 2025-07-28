namespace Booky_Store.API.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int Count { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
