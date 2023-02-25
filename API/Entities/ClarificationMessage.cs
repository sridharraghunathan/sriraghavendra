using System.Collections.Generic;
using System.Linq;
using MimeKit;
using static API.Entities.CommonItem;

namespace API.Entities
{
    public class ClarificationMessage
    {
        public ClarificationMessage(string fullName, IEnumerable<string> to, string subject, string question, string reply, string fromEmail, string password, EmailFlow type)
        {
            FullName = fullName;
            To.AddRange(to.Select(x => new MailboxAddress(x)));
            Subject = subject;
            Question = question;
            Reply = reply;
            FromEmail = fromEmail;
            Password = password;
            Type = type;
        }
        public string FullName { get; set; }
        public List<MailboxAddress> To { get; set; } = new List<MailboxAddress>();
        public string Subject { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public EmailFlow Type { get; set; }
        public string Question { get; set; }
        public string Reply { get; set; }
    }
}