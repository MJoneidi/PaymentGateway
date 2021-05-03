using System.ComponentModel.DataAnnotations;

namespace Payment.Domain.DTO.Requests
{
    public class UserLoginRequest
    {
        [Required]
        [RegularExpression(@"[\w-]+(\.?[\w-])*\@[\w-]+(\.[\w-]+)+")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
