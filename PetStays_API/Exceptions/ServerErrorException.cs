namespace PetStays_API.Exceptions
{
    public class ServerErrorException : Exception
    {
        public ServerErrorException(string message) : base(message)
        { }
    }
}
