namespace PetStays_API.Models
{
    public class PetDetail
    {
        public string Category { get; set; }

        public string Uid { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Gender { get; set; }

        public bool Vaccinated { get; set; }

        public string Color { get; set; }

        public string Breed { get; set; }

        public string Details { get; set; }

        public string Photo { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan TimeFrom { get; set; }

        public TimeSpan TimeTo { get; set; }

    }
}
