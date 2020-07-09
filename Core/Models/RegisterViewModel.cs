using System.ComponentModel.DataAnnotations;

namespace Vega.Core.Models
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name="Confirm Password")]
        [Compare("Password",ErrorMessage="Password and Confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}