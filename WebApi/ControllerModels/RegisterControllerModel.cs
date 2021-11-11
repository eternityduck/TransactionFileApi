using System.ComponentModel.DataAnnotations;

namespace TestProjectLegioSoft.ControllerModels
{
    public class RegisterControllerModel
    {
        [Required] 
        [Display(Name = "Email")] public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Field {0} must have minimum {2} and maximum {1} symbols.",
            MinimumLength = 5)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password doesn`t match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm the password")]
        public string PasswordConfirm { get; set; }
    }
}