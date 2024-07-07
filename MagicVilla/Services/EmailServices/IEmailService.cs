using MagicVilla.Models;
using MagicVilla.Models.DTO;

namespace MagicVilla.Services.EmailServices
{
    public interface IEmailService
    {
        void SendMail (MailDto request);
    }
}
