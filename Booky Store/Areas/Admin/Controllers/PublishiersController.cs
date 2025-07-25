using Microsoft.AspNetCore.Mvc;

namespace Booky_Store.API.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    public class PublishiersController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublishiersController(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet("Index")]
        public IActionResult Index()
        {
            var publishers = _unitOfWork.PublisherRepository.GetAsync().Result;
            if (publishers is null)
            {
                return NotFound("No publishers found.");
            }
            return Ok(publishers.Select(e => new
            {
                e.Id,
                e.Name,
                e.Description,
                e.IsBestSeller,
                e.SellsCount,
                e.LogoUrl
            }));
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CreatePublisherRequest createPublisher)
        {
            var publisher = createPublisher.Adapt<Author>();

            if (createPublisher.LogoUrl is not null && createPublisher.LogoUrl.Length > 0)
            {
                var fillName = Guid.NewGuid() + Path.GetExtension(createPublisher.LogoUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\PublisherLogo", fillName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await createPublisher.LogoUrl.CopyToAsync(stream);
                }
                publisher.ImageUrl = fillName;
            }

            await _unitOfWork.AuthorRepository.CreateAsync(publisher);
            return Ok("author Create Successfuly");
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var publisher = await _unitOfWork.AuthorRepository.GetOneAsync(e => e.Id == id);
            if (publisher is null)
            {
                return NotFound("Author not found.");
            }
            return Ok(publisher);
        }

        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] UpdataPublisherRequest updataPublisher)
        {
            var publisher = await _unitOfWork.PublisherRepository.GetOneAsync(e => e.Id == id);
            if (publisher is null)
            {
                return NotFound("Author not found.");
            }
            publisher.Name = updataPublisher.Name;
            publisher.Description = updataPublisher.Description;
            if (updataPublisher.LogoUrl is not null && updataPublisher.LogoUrl.Length > 0)
            {
                var fillName = Guid.NewGuid() + Path.GetExtension(updataPublisher.LogoUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\PublisherLogo", fillName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await updataPublisher.LogoUrl.CopyToAsync(stream);
                }

                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\PublisherLogo", publisher.LogoUrl!);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
                publisher.LogoUrl = fillName;
            }
            await _unitOfWork.PublisherRepository.UpdateAsync(publisher);
            return Ok("publisher Update Successfuly");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var publisher = await _unitOfWork.PublisherRepository.GetOneAsync(e => e.Id == id);
            if (publisher is null)
            {
                return NotFound("Author not found.");
            }
            await _unitOfWork.PublisherRepository.DeleteAsync(publisher);
            return Ok("author Delete Successfuly");
        }
    }
}
