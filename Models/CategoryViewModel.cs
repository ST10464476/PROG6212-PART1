using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class CategoryViewModel
    {
        public int CategoryID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required(ErrorMessage = "Please enter a category name.")]
        public string CategoryName { get; set; } = "";

        [Required]
        [Range(0.1, 1000)]
        public decimal Distance { get; set; }

        [Range(0, 120)]
        public int AgeMin { get; set; }

        [Range(0, 120)]
        public int AgeMax { get; set; }

        [Range(0, 100000)]
        public decimal EntryFee { get; set; }
    }
}