
using Microsoft.AspNetCore.Authorization;

namespace Booky_Store.API.Areas.Customer.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Customer")]
    [ApiController]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;
        

        public CartController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpPost("AddToCart")]
        public async Task<IActionResult> AddToCart(int bookId, int count)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Unauthorized("You must be logged in to add to cart.");
            }

            var book = _unitOfWork.BookRepository.Find(bookId);
            if (book is null)
            {
                return NotFound($"No book found with ID {bookId}.");
            }

            if (book.Quantity <= 0)
            {
                return BadRequest("This book is currently out of stock.");
            }

            var existingCartItem = await _unitOfWork.CartRepository.GetOneAsync(e =>
                e.ApplicationUserId == user.Id && e.BookId == bookId);

            if (existingCartItem is not null)
            {
                if (existingCartItem.Count + count > book.Quantity)
                {
                    return BadRequest("Requested quantity exceeds available stock.");
                }

                existingCartItem.Count += count;
            }
            else
            {
                await _unitOfWork.CartRepository.CreateAsync(new Cart
                {
                    BookId = bookId,
                    Count = count,
                    ApplicationUserId = user.Id
                });
            }

            await _unitOfWork.CartRepository.CommitAsync();
            return Ok("Book added to cart successfully.");
        }

        [HttpPost("Index")]
        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);

            if (user is not null)
            {
                var carts = await _unitOfWork.CartRepository.GetAsync(e => e.ApplicationUserId == user.Id,
                    includes: [m => m.Book]);
                var TotalPrice = carts.Sum(x => x.Book.Price * x.Count);
                return Ok(carts);
            }
            return NotFound();
        }

        [HttpPut("IncrementCount")]
        public async Task<IActionResult> IncrementCount(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                var booksInCart = await _unitOfWork.CartRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.BookId == bookId);
                if (booksInCart is not null)
                {
                    booksInCart.Count++;
                    await _unitOfWork.CartRepository.UpdateAsync(booksInCart);

                    return Ok("Item count IncrementCount successfully");
                }
                return NotFound();
            }
            return NotFound();

        }

        [HttpPut("DecrementCount")]
        public async Task<IActionResult> DecrementCount(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                var booksInCart = await _unitOfWork.CartRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.BookId == bookId);
                if (booksInCart is not null)
                {
                    if (booksInCart.Count > 1)
                    {
                        booksInCart.Count--;
                        await _unitOfWork.CartRepository.UpdateAsync(booksInCart);
                    }

                    return Ok("Item count decremented successfully");
                }
                return NotFound();
            }
            return NotFound();

        }

        public async Task<IActionResult> Delete(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                var booksInCart = await _unitOfWork.CartRepository.GetOneAsync(e => e.ApplicationUserId == user.Id && e.BookId == bookId);
                if (booksInCart is not null)
                {
                    await _unitOfWork.CartRepository.DeleteAsync(booksInCart);
                    await _unitOfWork.CartRepository.CommitAsync();

                    return Ok("Item removed from cart");
                }
                return NotFound();
            }
            return NotFound();
        }

        public async Task<IActionResult> Pay()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                var carts = await _unitOfWork.CartRepository.GetAsync(e => e.ApplicationUserId == user.Id, includes: [e => e.Book]);

                if (carts is not null)
                {

                    await _unitOfWork.OrderRepository.CreateAsync(new()
                    {
                        ApplicationUserId = user.Id,
                        Date = DateTime.UtcNow,
                        OrderStatus = OrderStatus.pending,
                        PaymentMethod = PaymentMethod.Visa,
                        TotalPrice = carts.Sum(e => e.Book.Price * e.Count)
                    });

                    var order = (await _unitOfWork.OrderRepository.GetAsync(e => e.ApplicationUserId == user.Id)).OrderBy(e => e.Id).LastOrDefault();

                    if (order is null)
                        return BadRequest();

                    var options = new SessionCreateOptions
                    {
                        PaymentMethodTypes = new List<string> { "card" },
                        LineItems = new List<SessionLineItemOptions>(),
                        Mode = "payment",
                        SuccessUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Success?orderId={order.Id}",
                        CancelUrl = $"{Request.Scheme}://{Request.Host}/Customer/Checkout/Cancel",
                    };


                    foreach (var item in carts)
                    {
                        options.LineItems.Add(new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                Currency = "egp",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = item.Book.Title,
                                    Description = item.Book.Description,
                                },
                                UnitAmount = (long)item.Book.Price * 100,
                            },
                            Quantity = item.Count,
                        });
                    }


                    var service = new SessionService();
                    var session = service.Create(options);
                    order.SessionId = session.Id;
                    await _unitOfWork.OrderRepository.CommitAsync();
                    return Ok(new { Url = session.Url });

                }
                return NotFound();
            }
            return NotFound();


        }
    }
}
