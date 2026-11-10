
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class EventViewModel
    {
        public int EventID { get; set; }

        public int OrganiserID { get; set; }

        [Required]
        public string EventName { get; set; } = "";

        [Required]
        public string Description { get; set; } = "";

        [Required]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required]
        public string Location { get; set; } = "";

        [Required]
        [Range(0.1, 1000)]
        public decimal Distance { get; set; }

        [Range(0, 100000)]
        public decimal EntryFee { get; set; }

        public string Status { get; set; } = "Upcoming";

        // Azure Blob Storage banner information
        public string? BannerImageUrl { get; set; }
    }
}
