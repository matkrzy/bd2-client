using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_client.Domain
{
    public class Category
    {
        [JsonProperty("categoryID")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("parentCategory")]
        public int? ParentId { get; set; }
    }
}
