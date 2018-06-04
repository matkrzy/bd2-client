using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BD_client.Domain
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

        public User()
        {

        }
    }
}
