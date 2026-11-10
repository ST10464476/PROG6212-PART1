using System.Collections.Generic;

namespace RaceDay.Web.Models
{
    public class EventDetailsViewModel
    {
        public EventViewModel Event { get; set; } = new EventViewModel();

        public List<CategoryViewModel> Categories { get; set; }
            = new List<CategoryViewModel>();
    }
}