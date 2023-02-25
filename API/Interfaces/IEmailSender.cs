using API.Entities;

namespace API.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail<T>(T message, string password = null, string fromEmail = null);
    }
}