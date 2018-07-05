using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BD_client.Dto
{
    public class Tag
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public String Name { get; set; }

        [JsonProperty("photoIds")]
        public List<long> PhotoID { get; set; }

        [JsonProperty("creationDate")]
        public String CreationDate { get; set; }

        public Tag() {
            PhotoID = new List<long>();
        }
    }
}
