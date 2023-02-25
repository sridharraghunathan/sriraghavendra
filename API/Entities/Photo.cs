using System;
using System.Text.Json.Serialization;

namespace API.Entities
{
    public class Photo
    {
        [JsonIgnore]
        public string PublicId { get; set; }
        public string Title { get; set; }
        public string EventDate { get; set; }
        public string ImageUrl { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }
    }
}