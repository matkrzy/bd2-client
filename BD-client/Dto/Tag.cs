using System;
using Newtonsoft.Json;

namespace BD_client.Dto
{
    public class Tag
    {
        [JsonProperty("tagID")]
        public long Id { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("photo")]
        public long PhotoID { get; set; }
        [JsonProperty("user")]
        public long UserID { get; set; }

        public long photo { get; set; }

        public Tag() { }
    }
}
