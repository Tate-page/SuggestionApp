using System.ComponentModel.DataAnnotations;


namespace SuggestionAppUI.Models
{
    public class loginUser
    {
        [Required]
        [StringLength(50, ErrorMessage = "DisplayName must be less than 50 characters")]
        public string DisplayName { get; set; }


        [Required]
        [StringLength(50, ErrorMessage = "Password must be less than 50 characters")]
        public string Password { get; set; }

    }
}
