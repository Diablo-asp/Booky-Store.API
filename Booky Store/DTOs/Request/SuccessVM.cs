namespace Booky_Store.API.DTOs.Request
{
    public class SuccessVM
    {
        public Order Order { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
