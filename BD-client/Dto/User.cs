using System;
using BD_client.Enums;
using Newtonsoft.Json;

namespace BD_client.Dto
{
    public class User
    {
        [JsonProperty("id")]
        public long id { get; set; }

        [JsonProperty("first_name")]
        public String FirstName { get; set; }

        [JsonProperty("last_name")]
        public String LastName { get; set; }

        [JsonProperty("email")]
        public String Email { get; set; }

        [JsonProperty("role")]
        public UserRole Role { get; set; }

        [JsonProperty("uuid")]
        public String uuid { get; set; }

        public User()
        {

        }
    }
}
