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
