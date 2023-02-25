using System;
using System.Text.Json.Serialization;

namespace API.Entities
{
    public class LatestEvents
    {
        public string Title { get; set; }
        public string EventDate { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }

    }
}