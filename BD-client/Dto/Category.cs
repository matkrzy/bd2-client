using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace BD_client.Dto
{
    public class Category
    {
        [Browsable(false)]
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [Browsable(false)]
        [JsonProperty("parentId")]
        public int? ParentId { get; set; }

        [Browsable(false)]
        [JsonProperty("creationDate")]
        public DateTime? CreationDate { get; set; }

        [Browsable(false)]
        [JsonProperty("userId")]
        public int UserId { get; set; }

        [Browsable(false)]
        [JsonProperty("user")]
        public User user { get; set; } = null;

        public Category()
        {
        }
    }
}