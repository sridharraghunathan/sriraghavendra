using System.Collections.Generic;
using System.Linq;
using MimeKit;
using static API.Entities.CommonItem;

namespace API.Entities
{
    public class Message
    {
        public Message(IEnumerable<string> to, string subject, string sevaName, int amountReceived, string address, string fullName, string fromEmail, string password, EmailFlow type)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            SevaName = sevaName;
            AmountReceived = amountReceived;
            Address = address;
            FullName = fullName;
            FromEmail = fromEmail;
            Password = password;
            Type = type;
        }
        public string FullName { get; set; }
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public int AmountReceived { get; set; }
        public string SevaName { get; set; }
        public string Address { get; set; }
        public EmailFlow Type { get; set; }

    }
}