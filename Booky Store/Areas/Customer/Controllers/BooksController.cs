using Microsoft.AspNetCore.Mvc;

namespace Booky_Store.API.Areas.Customer.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Customer")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks([FromForm] int? categoryId)
        {
            var booksQuery = _unitOfWork.BookRepository.GetQueryable()
                .Include(b => b.Publisher)
                .Include(b => b.BookAuthors)
                    .ThenInclude(ba => ba.Author);

            if (categoryId.HasValue)
            {
                booksQuery = (IIncludableQueryable<Book, Author>)booksQuery.Where(b => b.CategoryId == categoryId.Value);
            }

            var books = await booksQuery.ToListAsync();

            var result = books.Select(book => new
            {
                book.Id,
                book.Title,
                book.Description,
                book.Price,
                book.Review,
                book.Rate,
                book.CoverImageUrl,
                Publisher = new
                {
                    book.Publisher.Id,
                    book.Publisher.Name
                },
                Authors = book.BookAuthors.Select(ba => new
                {
                    ba.Author.Id,
                    ba.Author.Name
                })
            });

            return Ok(result);
        }
    }
}
