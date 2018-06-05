using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using BD_client.Enums;
using Newtonsoft.Json;

namespace BD_client.Dto
{
    public class Photo
    {
        [Browsable(false)]
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [Browsable(false)]
        [JsonProperty("userId")]
        public long UserId { get; set; }

        [Browsable(false)]
        [JsonProperty("uploadTime")]
        public DateTime UploadTime { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [Browsable(false)]
        [JsonProperty("shareState")]
        public ShareState ShareState { get; set; }

        [Browsable(false)]
        [JsonProperty("photoState")]
        public PhotoState PhotoState { get; set; }

        [Browsable(false)]
        [JsonProperty("rate")]
        public int Rate { get; set; }

        [Browsable(false)]
        [JsonProperty("tags")]
        public List<Tag> Tags { get; set; }

        [Browsable(false)]
        public int LikeCount { get; set; }

        [Browsable(false)]
        public BitmapFrame Image { get; set; }

        [Browsable(false)]
        [JsonProperty("url")]
        public String Url { get; set; }

        [Browsable(false)]
        [JsonProperty("path")]
        public String Path { get; set; }
        

        public Photo(){}
    }
}
