namespace PetStays_API.Exceptions
{
    public class BadReqException : Exception
    {
        public BadReqException(string message) : base(message)
        {
        }
    }
}