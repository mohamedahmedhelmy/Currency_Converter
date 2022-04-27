using System.ComponentModel.DataAnnotations;

namespace Core.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Please Enter Your Email")]
        [Display(Name = "Email Address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email ")]
       // [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter a Strong Password")]
        [Compare("ConfirmPassword", ErrorMessage = "Password does not match")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please Confirm Your Password")]
        [Display(Name = "ConfirmPassword")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
