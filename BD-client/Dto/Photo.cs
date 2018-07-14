using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using BD_client.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BD_client.Dto
{
    public class Photo
    {
        [Browsable(false)]
        [JsonProperty("categoryIds")]
        public List<int> CategoryIds { get; set; }

        [Browsable(false)]
        [JsonProperty("creationDate")]
        public DateTime CreationDate { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("description")] public string Description { get; set; } = "";

        [Browsable(false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [Browsable(false)]
        [JsonProperty("path")]
        public String Path { get; set; }

        [Browsable(false)]
        [JsonProperty("sharesIds")]
        public List<int> Shares { get; set; }

        [Browsable(false)]
        [JsonProperty("state")]
        [JsonConverter(typeof(StringEnumConverter))]
        public PhotoState PhotoState { get; set; }

        [Browsable(false)]
        [JsonProperty("url")]
        public String Url { get; set; }

        [Browsable(false)]
        [JsonProperty("userId")]
        public long UserId { get; set; }

        [Browsable(false)]
        [JsonProperty("visibility")]
        public PhotoVisibility ShareState { get; set; }

        [JsonProperty("tags")] public List<string> Tags { get; set; }

        [Browsable(false)]
        [JsonProperty("liked")]
        public bool Liked { get; set; }

        [Browsable(false)]
        [JsonProperty("likeIds")]
        public List<int> LikeIds { get; set; }

        [Browsable(false)]
        [JsonProperty("user")]
        public User User { get; set; }

        public Photo()
        {
        }
    }
}