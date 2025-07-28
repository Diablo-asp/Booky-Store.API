namespace Booky_Store.API.DTOs.Request
{
    public class UserBookRequest
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public string BookName { get; set; }
        public string BookImg { get; set; }
        public string BookISBN { get; set; }    
        public string OrderStatus { get; set; }  
        public string PublisherName { get; set; }  
        public string CategoryName { get; set; }
        
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
