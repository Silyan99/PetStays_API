using MediatR;
using PetStays.App.Models;

namespace PetStays.App.UserManagement.Command.Authenticate
{
    public class AuthenticateCommand : IRequest<AuthResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
