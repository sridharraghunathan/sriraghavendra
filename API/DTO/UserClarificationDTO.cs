using System;

namespace API.DTO
{
    public class UserClarificationDTO
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Question { get; set; }
    }
}