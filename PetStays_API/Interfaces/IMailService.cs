using PetStays_API.Models;

namespace PetStays_API.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
