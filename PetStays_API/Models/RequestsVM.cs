namespace PetStays_API.Models
{
    public class RequestsVM : PetDetail
    {
        public bool IsPaymentDone { get; set; }

        public bool Status { get; set; }

        public string Remarks { get; set; }
        public int OwnerId { get; set; }
        public int PetId { get; set; }
    }
}
