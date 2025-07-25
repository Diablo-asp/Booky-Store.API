using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Booky_Store.API.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    public class BooksController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var books = await _unitOfWork.BookRepository.GetAllBooksWithDetailsAsync();

            return Ok(books.Select(e => new
            {
                e.Id,
                e.Title,
                e.ISBN,
                e.Price,
                e.Rate,
                e.Review,
                e.Quantity,
                e.Description,
                e.CoverImageUrl,
                e.PublishDate,
                Category = e.Category.Name,
                Publisher = e.Publisher.Name,
                Authors = e.BookAuthors.Select(ba => ba.Author.Name).ToList()                
            }));
        }

        [HttpPost("Create")]    
        public async Task<IActionResult> Create([FromForm] CreateBookRequest createBookRequest)
        {
            var book = createBookRequest.Adapt<Book>();

            if(createBookRequest.CoverImageUrl is not null && createBookRequest.CoverImageUrl.Length > 0)
            {
                // Save the cover image to a directory and set the CoverImageUrl property
                var fillName = Guid.NewGuid() + Path.GetExtension(createBookRequest.CoverImageUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\BooksImg", fillName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await createBookRequest.CoverImageUrl.CopyToAsync(stream);
                }

                book.CoverImageUrl = fillName;
            }

            book.BookAuthors = createBookRequest.AuthorIds.Select(id => new BookAuthor
            {
                AuthorId = id
            }).ToList();

            await _unitOfWork.BookRepository.CreateAsync(book);

            return Ok("Book Has Been Created");
        }


        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var book = await _unitOfWork.BookRepository.GetOneAsync(e => e.Id == id);

            if (book is not null)
            {
                return Ok(book);
            }

            return NotFound("Book Not Found");
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] UpdataBookRequest updataBookRequest)
        {
            var bookInDB = await _unitOfWork.BookRepository.GetOneAsync(e => e.Id == id, tracked : false);
            var book = updataBookRequest.Adapt<Book>();


            if (bookInDB is not null)
            {
                if (updataBookRequest.CoverImageUrl is not null && updataBookRequest.CoverImageUrl.Length > 0)
                {
                    // Save the cover image to a directory and set the CoverImageUrl property
                    var fillName = Guid.NewGuid() + Path.GetExtension(updataBookRequest.CoverImageUrl.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\BooksImg", fillName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await updataBookRequest.CoverImageUrl.CopyToAsync(stream);
                    }

                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\BooksImg", bookInDB.CoverImageUrl!);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    book.CoverImageUrl = fillName;
                }
                else
                {
                    book.CoverImageUrl = bookInDB.CoverImageUrl;
                }

                await _unitOfWork.BookRepository.UpdateAsync(book);

                return Ok("Book Has Been Updated");
            }
            return NotFound("Book Not Found");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _unitOfWork.BookRepository.GetOneAsync(e => e.Id == id);

            if (book is not null)
            {
                // Delete old Img in wwwroot
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\BooksImg", book.CoverImageUrl!);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                await _unitOfWork.BookRepository.DeleteAsync(book);

                return Ok("Delete book Successfully");
            }

            return NotFound();
        }

    }
}
