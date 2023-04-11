namespace PetStays_API.Models
{
    public class Config
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Https { get; set; }
        public string ConnectionStrings { get; set; }
    }
}
