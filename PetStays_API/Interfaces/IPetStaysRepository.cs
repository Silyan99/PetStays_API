using PetStays_API.Models;

namespace PetStays_API.Interfaces
{
    public interface IPetStaysRepository
    {
        Task<Result> SignUp(Signup details);
        Task<Result> Login(Login details);
    }
}
