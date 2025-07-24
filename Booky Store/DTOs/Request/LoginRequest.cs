using System.ComponentModel.DataAnnotations;

namespace Booky_Store.API.DTOs.Request
{
    public class LoginRequest
    {
        [Required]
        public string UserNameOrEmail { get; set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
