using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Booky_Store.API.Areas.Customer.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Customer")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            Expression<Func<Book, object>>[] includes =
            {
                b => b.Category,
                b => b.Publisher
            };

            var allBooks = await _unitOfWork.BookRepository.GetAsync(includes : includes);

            var bestSellers = allBooks.OrderByDescending(b => b.Review).Take(5).ToList();
            var flashSale = allBooks.Where(b => b.Price < 30).Take(5).ToList();
            var recommended = allBooks.OrderByDescending(b => b.Rate).Take(5).ToList();

            var homePageRequest = new HomePageRequest
            {
                BestSellingBooks = bestSellers,
                FlashSaleBooks = flashSale,
                RecommendedBooks = recommended
            };


            return Ok(homePageRequest);
        }

        [HttpGet("Search")]
        public async Task<IActionResult> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Search keyword is required.");

            Expression<Func<Book, object>>[] includes =
            {
                b => b.Category,
                b => b.Publisher
            };

            var results = await _unitOfWork.BookRepository.GetAsync(
                b => b.Title.Contains(keyword) || (b.Description ?? "").Contains(keyword),
                includes
            );

            return Ok(results);
        }
    }
}
