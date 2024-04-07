using System.ComponentModel.DataAnnotations;

namespace GP.APIs.Dtos
{
    public class LoginDto
    {
        [Required] //Email feild is req
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        
        public string Password { get; set; }
    }
}
