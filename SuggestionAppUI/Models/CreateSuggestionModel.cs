using System.ComponentModel.DataAnnotations;

namespace SuggestionAppUI.Models
{
    public class CreateSuggestionModel
    {
        [Required]
        [MaxLength(length: 75)]
        public string Suggestion { get; set; }

        [Required]
        [MinLength(length: 1)]
        [Display(Name = "Category")]
        public string CateogryId{ get; set; }

        [MaxLength(length: 500)]
        public string Description { get; set; }

    }
}
