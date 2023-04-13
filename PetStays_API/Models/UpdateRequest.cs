using System.Text.Json.Serialization;

namespace PetStays_API.Models
{
    public class UpdateRequest
    {
        [JsonIgnore]
        public int Id { get; set; }
        public bool IsPaymentDone { get; set; }
        public bool Status { get; set; }
        public string Remarks { get; set; }
    }
}
