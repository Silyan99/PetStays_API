using MediatR;
using PetStays.App.Helpers;
using PetStays.App.Models;
using PetStays.Domain.Entities;
using PetStays.Persistence;
using System.Text;

namespace PetStays.App.UserManagement.Command.Authenticate
{
    public class AuthenticateHandler : IRequestHandler<AuthenticateCommand, AuthResult>
    {
        public readonly IPetRepository _petRepository;
        public AuthenticateHandler(IPetRepository petRepository)
        {
            _petRepository = petRepository;
        }

        public async Task<AuthResult> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            var base64EncodedPwd = Convert.FromBase64String(request.Password);
            string password = Encoding.UTF8.GetString(base64EncodedPwd);
            password = MD5GenHelper.MD5Hash(password);
            return await AuthenticateUser(request.Email, password);
        }

        private async Task<AuthResult> AuthenticateUser(string email, string password)
        {
            User user = await _petRepository.GetUserByEmail(email);

            if (user != null && (String.Compare(user.Email, email, StringComparison.Ordinal)) == 0 && user.Password.Equals(password))
            {
                return new AuthResult
                {
                    AuthToken = HelperFunction.GenerateToken(user, "WKsmlYl4EtR0ZoEeKWeSwKcz7ng", 2000)
                };
            }

            throw new Exception("Login failed");
        }

    }
}
