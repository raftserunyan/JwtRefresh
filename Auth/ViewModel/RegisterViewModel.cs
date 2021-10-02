using System.ComponentModel.DataAnnotations;

namespace Auth.ViewModel
{
	public class RegisterViewModel
    {  
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
