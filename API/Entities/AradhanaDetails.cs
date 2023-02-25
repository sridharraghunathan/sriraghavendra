using System;
using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class AradhanaDetails
    {
        public string Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }

        public string MobileNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }
        public string AdditionalInformation { get; set; }
        public string SevaName { get; set; }
        [Range(0.1, 100000, ErrorMessage = "Please proceed with Normal Bank Transaction To avoid Extra Charge")]
        public int Amount { get; set; }
        public string Year { get; set; } = DateTime.Now.Year.ToString();
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}