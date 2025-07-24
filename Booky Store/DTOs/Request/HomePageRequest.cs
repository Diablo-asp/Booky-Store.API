namespace Booky_Store.API.DTOs.Request
{
    public class HomePageRequest
    {
        public IEnumerable<Book> BestSellingBooks { get; set; } 
        public IEnumerable<Book> FlashSaleBooks { get; set; }
        public IEnumerable<Book> RecommendedBooks { get; set; }
    }
}
