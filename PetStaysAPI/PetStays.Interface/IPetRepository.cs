using PetStays.Domain.Entities;

namespace PetStays.Persistence
{
    public interface IPetRepository
    {
        Task<User> GetUserByEmail(string email);
    }
}
