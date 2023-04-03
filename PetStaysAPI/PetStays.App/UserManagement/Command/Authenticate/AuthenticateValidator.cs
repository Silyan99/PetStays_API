using FluentValidation;

namespace PetStays.App.UserManagement.Command.Authenticate
{
    public class AuthenticateValidator : AbstractValidator<AuthenticateCommand>
    {
        public AuthenticateValidator()
        {
            RuleFor(x => x.Email).EmailAddress().MaximumLength(256).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
