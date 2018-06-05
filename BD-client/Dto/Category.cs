using Newtonsoft.Json;

namespace BD_client.Dto
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
