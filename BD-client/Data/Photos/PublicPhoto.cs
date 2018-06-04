using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_client.Domain
{
    public class PublicPhoto : Photo
    {
        [JsonProperty("rate")]
        public int Rate { get; set; }
    }
}
