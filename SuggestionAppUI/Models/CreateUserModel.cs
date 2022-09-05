using System.ComponentModel.DataAnnotations;

namespace SuggestionAppUI.Models
{
    public class CreateUserModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Name must be less than 50 characters")]
        [RegularExpression(@"^[a-zA-Z\s.\-]{2,}$", ErrorMessage = "Display Name must contain only alphbetical letters, '.', and '-'.")]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Password must be less than 50 characters")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])(?=.*[^\w\d\s:])([^\s]){8,32}$", ErrorMessage = "Password doesnt meet security rules")]
        public string Password1 { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Password must be less than 50 characters")]
        [Compare("Password1")]
        public string Password2 { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s.\-]{2,}$", ErrorMessage = "Favourite color must contain only alphbetical letters, '.', and '-'.")]
        public string favoriteColor { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s.\-]{2,}$", ErrorMessage = "First Name must contain only alphbetical letters, '.', and '-'.")]
        public string Fname { get; set; }

        [Required]
        [RegularExpression(@"^[a-zA-Z\s.\-]{2,}$", ErrorMessage = "Last Name must contain only alphbetical letters, '.', and '-'.")]
        public string Lname { get; set; }

        [Required]
        [RegularExpression(@"^((?!\.)[\w-_.]*[^.])(@\w+)(\.\w+(\.\w+)?[^.\W])$", ErrorMessage = "Email is invalid.")]
        public string email { get; set; }
    }
}
