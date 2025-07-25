

using Booky_Store.API.Models;

namespace Booky_Store.API.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    public class AuthorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var authors = _unitOfWork.AuthorRepository.GetAsync();

            if (authors is null)
            {
                return NotFound("No authors found.");
            }
            return Ok(authors);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CreateAuthorRequest createAuthor)
        {
            var author = createAuthor.Adapt<Author>();

            if (createAuthor.ImageUrl is not null && createAuthor.ImageUrl.Length > 0)
            {
                var fillName = Guid.NewGuid() + Path.GetExtension(createAuthor.ImageUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\AuthorImg", fillName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await createAuthor.ImageUrl.CopyToAsync(stream);
                }
                author.ImageUrl = fillName;
            }

            await _unitOfWork.AuthorRepository.CreateAsync(author);
            return Ok("author Create Successfuly");
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var author = await _unitOfWork.AuthorRepository.GetOneAsync(e => e.Id == id);
            if (author is null)
            {
                return NotFound("Author not found.");
            }
            return Ok(author);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] UpdateAuthorRequest updateAuthor)
        {
            var author = await _unitOfWork.AuthorRepository.GetOneAsync(e => e.Id == id);
            if (author is null)
            {
                return NotFound("Author not found.");
            }
            author.Name = updateAuthor.Name;
            author.Bio = updateAuthor.Bio;
            if (updateAuthor.ImageUrl is not null && updateAuthor.ImageUrl.Length > 0)
            {
                var fillName = Guid.NewGuid() + Path.GetExtension(updateAuthor.ImageUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\AuthorImg", fillName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await updateAuthor.ImageUrl.CopyToAsync(stream);
                }

                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\AuthorImg", author.ImageUrl!);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
                author.ImageUrl = fillName;
            }
            await _unitOfWork.AuthorRepository.UpdateAsync(author);
            return Ok("author Update Successfuly");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var author = await _unitOfWork.AuthorRepository.GetOneAsync(e => e.Id == id);
            if (author is null)
            {
                return NotFound("Author not found.");
            }
            await _unitOfWork.AuthorRepository.DeleteAsync(author);
            return Ok("author Delete Successfuly");
        }
    }
}
