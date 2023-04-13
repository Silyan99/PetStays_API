namespace PetStays_API.Models
{
    public class RequestDetail
    {
        public int MadeBy { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan TimeFrom { get; set; }

        public TimeSpan TimeTo { get; set; }

        public int PetId { get; set; }

        public bool IsPaymentDone { get; set; }

        public bool Status { get; set; }

        public string Remarks { get; set; }
    }
}
