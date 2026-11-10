using System;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Web.Models
{
    public class ResultViewModel
    {
        public int ResultID { get; set; }

        [Required]
        public int EnrolmentID { get; set; }

        public TimeSpan FinishTime { get; set; }

        [Range(1, int.MaxValue)]
        public int Position { get; set; }

        public decimal AveragePace { get; set; }

        public string ResultStatus { get; set; } = "";
    }
}