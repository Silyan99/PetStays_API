using PetStays_API.Models;

namespace PetStays_API.Interfaces
{
    public interface IPetStaysRepository
    {
        Task<Result> SignUpUser(Signup details);
        Task<Result> SignUpAdmin(Signup details);
        Task<AuthVM> Login(Login details);
        Task<Result> Contact(ContactInfo details);
    }
}
