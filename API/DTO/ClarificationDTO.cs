using System.Collections.Generic;
using MimeKit;
using static API.Entities.CommonItem;

namespace API.DTO
{
    public class ClarificationDTO
    {
        public string FullName
        {
            get; set;
        }
        public string[] To { get; set; }
        public string Subject { get; set; }
        public string Question { get; set; }
        public string Reply { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public EmailFlow Type { get; set; }
    }
}