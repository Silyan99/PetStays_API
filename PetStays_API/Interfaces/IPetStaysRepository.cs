using PetStays_API.Models;
using System.Security.Claims;

namespace PetStays_API.Interfaces
{
    public interface IPetStaysRepository
    {
        Task<Result> SignUpUser(Signup details);
        //Task<Result> SignUpAdmin(Signup details);
        Task<AuthVM> Login(Login details);
        Task<Result> Contact(ContactInfo details);
        Task<List<RequestsVM>> GetAllRequest();
        Task<RequestsVM> GetRequest(int id);
        Task<List<RequestsVM>> GetAllUserRequest(ClaimsIdentity identity);
        Task<RequestsVM> GetUserRequest(int id, ClaimsIdentity identity);
        Task<Result> UpdateRequest(UpdateRequest details);
        Task<Result> DeleteRequest(int id);
        //Task<Result> AddPet(PetDetail details);
        Task<Result> AddRequest(PetDetail details, ClaimsIdentity identity);
        Task<Result> AddAvailability(AvailabilityDetail details);
        Task<List<AvailabilityDetail>> GetAvailability(int id);
        Task<List<AvailabilityDetail>> GetAdminAvailability();
        Task<Result> DeleteAvailability(int id);
        Task<UserDetail> GetUserById(int id);
    }
}
