using System.Text.Json.Serialization;

namespace PetStays_API.Models
{
    public class AvailabilityDetail
    {
        public string Date { get; set; }

        public string TimeStart { get; set; }

        public string TimeEnd { get; set; }

        public bool FullDay { get; set; }

        public int AdminId { get; set; }

        public int Id { get; set; }
    }
}
