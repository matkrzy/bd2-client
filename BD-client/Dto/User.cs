using System;
using BD_client.Enums;
using Newtonsoft.Json;

namespace BD_client.Dto
{
    public class User
    {
        [JsonProperty("userID")]
        public long id { get; set; }
        [JsonProperty("firstName")]
        public String FirstName { get; set; }
        [JsonProperty("lastName")]
        public String LastName { get; set; }
        [JsonProperty("email")]
        public String Email { get; set; }
        [JsonProperty("password")]
        public String Password { get; set; }
        [JsonProperty("role")]
        public UserState? Role { get; set; }

        [JsonProperty("uuid")]
        public String uuid { get; set; }

        public User()
        {

        }
    }
}
