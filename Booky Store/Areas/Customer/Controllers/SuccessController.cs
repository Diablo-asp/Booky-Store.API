using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Booky_Store.API.Areas.Customer.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Customer")]
    [ApiController]
    [Authorize]
    public class SuccessController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public SuccessController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet("Success")]
        public async Task<IActionResult> Success(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetOneAsync(e => e.Id == orderId);

            if (order is null)
                return NotFound();

            order.OrderStatus = OrderStatus.Paid;

            var service = new SessionService();
            var session = service.Get(order.SessionId);

            order.PaymentId = session.PaymentIntentId;

            await _unitOfWork.OrderRepository.CommitAsync();

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound();

            var carts = await _unitOfWork.CartRepository.GetAsync(e => e.ApplicationUserId == user.Id, includes: [e => e.Book]);

            List<OrderItem> orderItems = new();
            foreach (var item in carts)
            {
                orderItems.Add(new()
                {
                    BookId = item.BookId,
                    OrderId = orderId,
                    Quantity = item.Count,
                    TotalPrice = (decimal)(item.Book.Price * item.Count)
                });
                var movie = _unitOfWork.BookRepository.Find(item.BookId);
                if (movie is not null)
                {
                    movie.Quantity -= item.Count;
                }
            }

            await _unitOfWork.OrderItemRepository.CreateRangeAsync(orderItems);

            foreach (var item in carts)
            {
                await _unitOfWork.CartRepository.DeleteAsync(item);
            }

            await _unitOfWork.CommitAsync();

            var orders = await _unitOfWork.OrderItemRepository.GetAsync(
                e => e.OrderId == orderId,
                includes: [e => e.Book]);

            return Ok(new { message = "Order confirmed successfully", orderId = order.Id });
        }
    }
}
