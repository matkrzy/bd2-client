using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Deserializers;

namespace BD_client.Api.Utils
{
    class Utils
    {
        public T Deserialize<T>(IRestResponse data)
        {
            JsonDeserializer deserial = new JsonDeserializer();

            return deserial.Deserialize<T>(data);
        }
    }
}
