using System.ComponentModel.DataAnnotations;

namespace BlogCore.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("Password", ErrorMessage="Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}