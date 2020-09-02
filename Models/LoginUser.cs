using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NonProfit.Models
{
    public class LoginUser
    {
        [NotMapped]
        [Required(ErrorMessage = "is required.")]
        [EmailAddress]
        [Display(Name = "Login Email")]
        public string LoginEmail {get; set;}

        [Required(ErrorMessage = "is required")]
        [MinLength(8, ErrorMessage = "must be at least 8 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]

        public string LoginPassword {get; set;}
    }
}