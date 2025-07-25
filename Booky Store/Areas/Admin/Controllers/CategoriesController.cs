using System.Threading.Tasks;
using Booky_Store.API.DTOs.Request;
using Booky_Store.API.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Booky_Store.API.Areas.Admin.Controllers
{
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("Index")]
        public async Task<IActionResult> Index()
        {
            var result = await _unitOfWork.CategoryRepository.GetAsync();
            if (result == null || !result.Any())
            {
                return NotFound("No categories found.");
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CategoryRequest categoryRequest)
        {
            var category = categoryRequest.Adapt<Category>();

            if (categoryRequest.ImgUrl is not null && categoryRequest.ImgUrl.Length > 0)
            {
                var fillName = Guid.NewGuid() + Path.GetExtension(categoryRequest.ImgUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\CategoryImg", fillName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await categoryRequest.ImgUrl.CopyToAsync(stream);
                }

                category.ImgUrl = fillName;
            }

            await _unitOfWork.CategoryRepository.CreateAsync(category);

            return Ok(category);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetOneAsync(e => e.Id == id);
            if (category is null)
            {
                return NotFound($"Category with ID {id} not found.");
            }
            return Ok(category);
        }


        [HttpPut("Edit/{id}")]
        public async Task<IActionResult> Edit([FromRoute] int id, [FromForm] UpdataCategoryRequest updataCategory)
        {
            var category = await _unitOfWork.CategoryRepository.GetOneAsync(e => e.Id == id);

            if (category is null)
            {
                return NotFound($"Category with ID {id} not found.");
            }
           
           

            category.Name = updataCategory.Name;
            if (updataCategory.ImgUrl is not null && updataCategory.ImgUrl.Length > 0)
            {
                var fillName = Guid.NewGuid() + Path.GetExtension(updataCategory.ImgUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\CategoryImg", fillName);
                using (var stream = System.IO.File.Create(filePath))
                {
                    await updataCategory.ImgUrl.CopyToAsync(stream);
                }

                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\CategoryImg", category.ImgUrl!);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
                category.ImgUrl = fillName;
            }
            else
            {
                category.ImgUrl = category.ImgUrl;
            }

            await _unitOfWork.CategoryRepository.UpdateAsync(category);
            return Ok("Category has been updated.");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _unitOfWork.CategoryRepository.GetOneAsync(e => e.Id == id);

            if (category is not null)
            {
                // Delete old Img in wwwroot
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\CategoryImg", category.ImgUrl!);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                await _unitOfWork.CategoryRepository.DeleteAsync(category);

                return Ok("Delete category Successfully");
            }

            return NotFound();
        }

    }
}
