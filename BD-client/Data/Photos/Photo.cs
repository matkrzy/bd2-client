using BD_client.Data.Photos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BD_client.Domain
{


    public class Photo
    {
        [Browsable(false)]
        [JsonProperty("photoID")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [Browsable(false)]
        [JsonProperty("owner_email")]
        public string UserEmail { get; set; }
        [Browsable(false)]
        [JsonProperty("uploadTime")]
        public DateTime UploadTime { get; set; }
        [Browsable(false)]
        [JsonProperty("description")]
        public string Description { get; set; }
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
        public int LikeCount { get { return 45; } }

        [Browsable(false)]
        public BitmapFrame Image { get; set; }
        [Browsable(false)]
        public Uri Uri { get; set; }
        [Browsable(false)]
        public long UserID { get; set; }
        [Browsable(false)]
        [JsonProperty("path")]
        public String Path { get; set; }

        public Photo(string path, int id)
        {
            Id = id;
            Path = path;
            Uri = new Uri(path);

        }

        public Photo()
        {

        }
    }
}
