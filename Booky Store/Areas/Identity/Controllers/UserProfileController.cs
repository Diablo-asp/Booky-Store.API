using Booky_Store.API.DTOs.Request;
using Booky_Store.API.Repositories.IRepositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Booky_Store.API.Areas.Identity.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Identity")]
    [ApiController]
    public class UserProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        [HttpGet("GetUserProfile")]
        public async Task<IActionResult> GetUserProfile()
        {
            var jwtAuth = await HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
            if (!jwtAuth.Succeeded)
                return Unauthorized(new { message = "JWT token is missing or invalid" });

            var user = await _userManager.GetUserAsync(User);
            if (user is null && jwtAuth is null)
                return NotFound();

            var getUser = new UserProfileRequest
            {
                UserName = user!.UserName!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                Email = user.Email!,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                ProfilePicture = user.ProfilePicture 
            };

            return Ok(getUser);
        }

        //[HttpGet]
        //public async Task<IActionResult> Edit()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null) return NotFound();

        //    var modle = new UserProfileRequest
        //    {
        //        UserName = user.UserName!,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName,
        //        Email = user.Email!,
        //        PhoneNumber = user.PhoneNumber,
        //        Address = user.Address,
        //        ProfilePicture = user.ProfilePicture
        //    };

        //    return View(modle);
        //}

        [HttpPost("Edit")]
        public async Task<IActionResult> Edit(UserProfileRequest userProfileRequest, IFormFile? ImgUrl)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
                return NotFound();

            // تحديث البيانات
            user.FirstName = userProfileRequest.FirstName;
            user.LastName = userProfileRequest.LastName;
            user.PhoneNumber = userProfileRequest.PhoneNumber;
            user.Address = userProfileRequest.Address;
            // تغيير الصورة لو موجودة
            if (ImgUrl != null && ImgUrl.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(ImgUrl.FileName);
                var filePath = Path.Combine("wwwroot/images/UserProfilePic", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImgUrl.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    var oldImagePath = Path.Combine("wwwroot/images/UserProfilePic", user.ProfilePicture);
                    if (System.IO.File.Exists(oldImagePath))
                        System.IO.File.Delete(oldImagePath);
                }

                user.ProfilePicture = fileName;
            }


            // تغيير الباسورد لو مطلوب
            if (!string.IsNullOrWhiteSpace(userProfileRequest.OldPassword) && !string.IsNullOrWhiteSpace(userProfileRequest.NewPassword))
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, userProfileRequest.OldPassword);

                if (!passwordCheck)
                    return BadRequest("Old password is incorrect");
                

                var result = await _userManager.ChangePasswordAsync(user, userProfileRequest.OldPassword, userProfileRequest.NewPassword);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);
                
            }
            await _userManager.UpdateAsync(user);           
            return Ok("Profile updated successfully!");
        }

        //[HttpGet("MyBooks")]
        //public async Task<IActionResult> MyBooks()
        //{
        //    var user = await _userManager.GetUserAsync(User);

        //    if (user is null)
        //        return NotFound();

        //    var orders = await _unitOfWork.OrderRepository.GetAsync(
        //        o => o.ApplicationUserId == user.Id && o.OrderStatus == OrderStatus.Paid
        //    );



        //    var tickets = new List<UserTicketRequest>();

        //    foreach (var order in orders)
        //    {
        //        var orderItems = await _unitOfWork.OrderItemRepository.GetAsync(
        //            oi => oi.OrderId == order.Id,
        //            includes: [oi => oi.Movie, oi => oi.Movie.cenima, oi => oi.Movie.Category]
        //        );

        //        foreach (var item in orderItems)
        //        {
        //            tickets.Add(new UserTicketRequest
        //            {
        //                OrderId = order.Id,
        //                OrderDate = order.Date,

        //                MovieName = item.Movie.Name,
        //                MovieImage = item.Movie.ImgUrl!,
        //                MovieStatus = item.Movie.CurrentStatus.ToString(),
        //                StartDate = item.Movie.StartDate,
        //                EndDate = item.Movie.EndDate,
        //                CinemaName = item.Movie.cenima.Name,
        //                CategoryName = item.Movie.Category.Name,
        //                OrderStatus = item.Order.OrderStatus.ToString(),

        //                Quantity = item.Quantity, // أو item.Count لو عندك Count
        //                TotalPrice = item.TotalPrice
        //            });
        //        }
        //    }

        //    return View(tickets);
        //}
    }
}
