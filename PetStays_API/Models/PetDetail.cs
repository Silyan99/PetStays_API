using System.Text.Json.Serialization;

namespace PetStays_API.Models
{
    public class PetDetail
    {
        public int Id { get; set; }
        public string Category { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public bool Vaccinated { get; set; }

        public string Color { get; set; }

        public string Breed { get; set; }

        public string Details { get; set; }

        public IFormFile PhotoFile { get; set; }
        public string Photo { get; set; }

        public string DateFrom { get; set; }

        public string DateTo { get; set; }

        public string TimeFrom { get; set; }

        public string TimeTo { get; set; }

    }
}
