using Microsoft.EntityFrameworkCore;
using PetStays.Domain.Entities;
using PetStays.Persistence;

namespace PetStays.SQL
{
    public class PetRepository : IPetRepository
    {
        public readonly PetStaysContext _ctx;

        public PetRepository(string connString)
        {
            _ctx = new PetStaysContext(connString);
        }
        public async Task<User> GetUserByEmail(string email)
        {
            return await _ctx.Users.Where(u => u.Email == email).FirstOrDefaultAsync();
        }
    }
}
