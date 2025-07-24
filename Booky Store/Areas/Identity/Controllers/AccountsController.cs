using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace Booky_Store.API.Areas.Identity.Controllers
{
    [Route("api/[area]/[controller]")]
    [Area("Identity")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AccountsController(UserManager<ApplicationUser> userManager,
            IEmailSender emailSender, SignInManager<ApplicationUser> signInManager)
        {
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // Register Action

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerRequest.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "This email is already registered.");
            }

            //ApplicationUser user = new()
            //{
            //    UserName = registerVM.UserName,
            //    Email = registerVM.Email,
            //    FirstName = registerVM.FirstName,
            //    LastName = registerVM.LastName,
            //    Address = registerVM.Address
            //};
            var user = registerRequest.Adapt<ApplicationUser>();

            var result = await _userManager.CreateAsync(user, registerRequest.Password);


            if (result.Succeeded)
            {
                // login the user
                await _userManager.AddToRoleAsync(user, "Customer");

                //send Confirmation Email
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var link = Url.Action(nameof(ConfirmEmail),
                    "Account", new { userId = user.Id, token, area = "Identity" }, Request.Scheme);

                await _emailSender.SendEmailAsync(user!.Email ?? "",
                    "Confirm Your Account", $"<h1>Confirm Your Account By Clicking <a href='{link}'>here</a></h1>");

                user.EmailConfirmationSentAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                return Ok("Create User Successfully");
            }
            return BadRequest(result.Errors);
        }


        // Login Action
        
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest LoginRequset)
        {
            var user = await _userManager.FindByEmailAsync(LoginRequset.UserNameOrEmail)
                       ?? await _userManager.FindByNameAsync(LoginRequset.UserNameOrEmail);

            if (user is not null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, LoginRequset.Password, true);

                if (result.Succeeded)
                {
                    if (!user.EmailConfirmed)
                    {
                        return BadRequest("Confirm Your Email");
                    }

                    if (!user.LockoutEnabled)
                    {
                        return BadRequest($"You have block till {user.LockoutEnd}");
                    }
                    var roles = await _userManager.GetRolesAsync(user);

                    var claims = new List<Claim> {
                        new Claim(ClaimTypes.NameIdentifier, user.Id),
                        new Claim(ClaimTypes.Name, user.UserName!),
                        new Claim(ClaimTypes.Email, user.Email!),
                        new Claim(ClaimTypes.Role, String.Join(" ", roles))
                    };

                    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("EraaSoft514##EraaSoft514##EraaSoft514##"));

                    var signInCredential = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "https://localhost:7231",
                        audience: "https://localhost:4200,https://localhost:5000",
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(3),
                        signingCredentials: signInCredential
                    );
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
                else if (result.IsLockedOut)
                {
                    return BadRequest("Too Many attempts, try again after 5 min");
                }
            }
                return BadRequest("Invalid username OR Password");
        }


        // SignOut Action
        [HttpPost("SignOut")]
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();            
            return Ok("Logout Successfully");
        }

        // ConfirmEmail Action
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is not null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    return Ok("Confirm Email Successfully");
                }
                else
                {
                    return BadRequest($"{String.Join(",", result.Errors)}");
                }
            }

            return NotFound();
        }


        // Resend Condirm Email
       
        [HttpPost("ResendEmailConfirmation")]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationRequest resendEmailConfirmationRequest)
        {
            if (!ModelState.IsValid)
            {
                return View(resendEmailConfirmationRequest);
            }

            var user = await _userManager.FindByEmailAsync(resendEmailConfirmationRequest.EmailOrUserName)
                       ?? await _userManager.FindByNameAsync(resendEmailConfirmationRequest.EmailOrUserName);

            if (user is null)
                return BadRequest("Invalid username Or Email");
            

            // Check if already confirmed
            if (user.EmailConfirmed)
            {
                return BadRequest("Your email is already confirmed");
            }

            // Check resend delay (15 minutes)
            if (user.EmailConfirmationSentAt.HasValue &&
                user.EmailConfirmationSentAt.Value.AddMinutes(15) > DateTime.UtcNow)
            {
                var remainingMinutes = (user.EmailConfirmationSentAt.Value.AddMinutes(15) - DateTime.UtcNow).Minutes;
                return BadRequest($"Please wait {remainingMinutes} minute(s) before requesting a new confirmation email");
            }

            // Send new confirmation email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action(nameof(ConfirmEmail), "Account", new { userId = user.Id, token, area = "Identity" }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email!, "Confirm Your Account Again!",
                $"<h1>Click <a href='{link}'>here</a> to confirm your account.</h1>");

            // Update sent time
            user.EmailConfirmationSentAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return Ok("Confirmation email sent successfully.");
        }

        // Forget Password Action
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordRequest forgetPasswordRequest)
        {
            var user = await _userManager.FindByEmailAsync(forgetPasswordRequest.EmailOrUserName);

            if (user is null)            
                return BadRequest("User not found.");
            

            if (!user.EmailConfirmed) 
                return BadRequest("Your email needs to be confirmed first.");
            

            // Check resend delay (15 minutes)

            if (user.EmailConfirmationSentAt.HasValue &&
                user.EmailConfirmationSentAt.Value.AddMinutes(10) > DateTime.UtcNow)
            {
                var remainingMinutes = (user.EmailConfirmationSentAt.Value.AddMinutes(10) - DateTime.UtcNow).Minutes;                
                return BadRequest($"Please wait {remainingMinutes} minute(s) before requesting a new confirmation email.");
            }

            // Generate token and encode it
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = Url.Action(nameof(ChangePassword), "Account",
                new { userId = user.Id, token, area = "Identity" }, Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email!, "Change Your Password!",
                $"<h1>Click <a href='{link}'>here</a> to Reset your Password.</h1>");

            // Save time of last request (optional) 
            user.EmailConfirmationSentAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            TempData["success-notification"] = "Reset link sent to your email successfully.";

            return Ok("Reset link sent to your email successfully");
        }


        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            var user = await _userManager.FindByIdAsync(changePasswordRequest.UserId);

            if (user is not null)
            {
                var result = await _userManager.ResetPasswordAsync(user, token: changePasswordRequest.Token, changePasswordRequest.Password);

                if (result.Succeeded)
                {
                    // Save the DateTime Password Changed
                    user.PasswordLastChangedAt = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);

                    return Ok("Password changed successfully");
                }
                return BadRequest(result.Errors);
            }
            return NotFound();
        }
    }
}
