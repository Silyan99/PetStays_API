namespace PetStays_API.Models
{
    public class AvailabilityDetail
    {
        public DateTime Date { get; set; }

        public TimeSpan TimeStart { get; set; }

        public TimeSpan TimeEnd { get; set; }

        public bool FullDay { get; set; }

        public int AdminId { get; set; }
    }
}
