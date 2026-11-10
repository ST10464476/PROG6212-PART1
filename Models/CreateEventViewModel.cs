
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class CreateEventViewModel
    {
        [Required(ErrorMessage = "Please enter the event name.")]
        public string EventName { get; set; } = "";

        [Required(ErrorMessage = "Please enter the event description.")]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Please select the event date.")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Please enter the event location.")]
        public string Location { get; set; } = "";

        [Required]
        [Range(0.1, 1000, ErrorMessage = "Distance must be between 0.1 and 1000 km.")]
        public decimal Distance { get; set; }

        [Range(0, 100000, ErrorMessage = "Entry fee cannot be negative.")]
        public decimal EntryFee { get; set; }
    }
}
