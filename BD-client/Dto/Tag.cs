using System;
using Newtonsoft.Json;

namespace BD_client.Dto
{
    public class Tag
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("photo_id")]
        public long PhotoID { get; set; }

        public Tag() { }
    }
}
