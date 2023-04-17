namespace PetStays_API.Models
{
    public class RequestsVM : PetDetail
    {
        public bool IsPaymentDone { get; set; }

        public string Status { get; set; }

        public string Remarks { get; set; }
        public int OwnerId { get; set; }
        public int PetId { get; set; }
        public string OwnerName { get; set; }
        public string Address { get; set; }
    }
}
