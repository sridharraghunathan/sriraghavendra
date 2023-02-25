using System;
using Microsoft.AspNetCore.Http;

namespace API.DTO
{
    public class LatestEventsDTO
    {
        public string Title { get; set; }
        public string EventDate { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}