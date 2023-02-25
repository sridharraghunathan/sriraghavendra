using System.Collections.Generic;
using static API.Entities.CommonItem;

namespace API.DTO
{
    public class MessageDTO
    {
        public string[] To  { get; set; }
        public string Subject { get; set; }
        public string SevaName { get; set; }
        public int AmountReceived { get; set; }
        public string Address { get; set; }
        public string FullName { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public EmailFlow Type { get; set; }
    }
}