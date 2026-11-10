
using System;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class EnrolmentViewModel
    {
        public int EnrolmentID { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        public int CategoryID { get; set; }

        public int ParticipantID { get; set; }

        public DateTime EnrolmentDate { get; set; }

        public int RaceNumber { get; set; }

        public string PaymentStatus { get; set; } = "";

        public string EventName { get; set; } = "";

        public string CategoryName { get; set; } = "";
    }
}
