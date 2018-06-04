using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_client.Domain
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

    }
}
