using System;
using Microsoft.AspNetCore.Http;

namespace API.DTO
{
    public class PhotoDto
    {
        public string Title { get; set; }
        public string EventDate { get; set; }
        public IFormFile Image { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}