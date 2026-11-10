namespace RaceDay.Web.Models
{
    public class ParticipantProfileViewModel
    {
        public int UserID { get; set; }

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Email { get; set; } = "";

        public string PhoneNumber { get; set; } = "";
    }
}