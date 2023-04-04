using PetStays_API.DBModels;
using PetStays_API.Interfaces;
using PetStays_API.Models;

namespace PetStays_API.Repositories
{
    public class PetStaysRepository : IPetStaysRepository
    {
        public PetStaysRepository() { }

        public async Task<Result> SignUp(Signup details)
        {
            Result result = new Result();
            try
            {
                using (PetStaysContext con = new PetStaysContext())
                {
                    var user = new User()
                    {
                        Email = details.Email,
                        Password = details.Password,
                        FullName = details.FullName,
                        Mobile = details.Mobile
                    };
                    con.Users.Add(user);

                    con.SaveChanges();
                    result.Status = true;
                    result.Message = "User added successfully";
                }
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.Message = ex.Message;
            }

            return result;

        }

        public async Task<Result> Login(Login details)
        {
            Result result = new Result();
            return result;
        }
    }
}
