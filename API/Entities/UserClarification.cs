using System;

namespace API.Entities
{
    public class UserClarification
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Question { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}