using System.ComponentModel.DataAnnotations;

namespace Booky_Store.API.DTOs.Request
{
    public class ForgetPasswordRequest
    {
        [Required]
        public string EmailOrUserName { get; set; }
    }
}
