using System.ComponentModel.DataAnnotations;

namespace Booky_Store.API.DTOs.Request
{
    public class ResendEmailConfirmationRequest
    {
        [Required]
        public string EmailOrUserName { get; set; }
    }
}
