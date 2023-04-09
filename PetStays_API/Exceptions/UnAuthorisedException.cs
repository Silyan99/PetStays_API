namespace PetStays_API.Exceptions
{
    public class UnAuthorisedException : Exception
    {
        public UnAuthorisedException(string message) : base(message)
        {
        }
    }
}