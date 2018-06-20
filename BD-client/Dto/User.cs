using System;
using System.ComponentModel;
using BD_client.Enums;
using Newtonsoft.Json;

namespace BD_client.Dto
{
    public class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("firstName")]
        public String FirstName { get; set; }

        [JsonProperty("lastName")]
        public String LastName { get; set; }

        [JsonProperty("email")]
        public String Email { get; set; }

        [JsonProperty("role")]
        public UserRole Role { get; set; }

        [JsonProperty("uuid")]
        public String Uuid { get; set; }

        [JsonProperty("creationDate")]
        public String CreationDate { get; set; }

        [JsonProperty("password")]
        public String Password { get; set; }

        public User()
        {

        }
    }
}
