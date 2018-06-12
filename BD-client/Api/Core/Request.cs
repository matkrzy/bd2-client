using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using RestSharp;

namespace BD_client.Api.Core
{
    enum AuthorizationType
    {
        Cookies,
        Header
    };

    class Request
    {
        private static String apiHost = ConfigurationManager.AppSettings["ApiHost"];
        private static String apiPath = ConfigurationManager.AppSettings["ApiPath"];
        private static String apiUrl = apiHost + apiPath;
        private static String JWT = ConfigurationManager.AppSettings["JWT"];
        private AuthorizationType authType = AuthorizationType.Cookies;

        private RestClient client = null;
        private RestRequest request = null;

        private void AddJwtToken()
        {
            Cookie cookie = new Cookie
            {
                Name = "JWT",
                Value = JWT,
                Domain = new Uri(apiUrl).Host,
            };

            this.client.CookieContainer = new CookieContainer();
            this.client.CookieContainer.Add(cookie);
        }

        public Request(String endpoint)
        {
            this.client = new RestClient(apiUrl);
            this.request = new RestRequest(endpoint);
            this.AddJwtToken();
        }

        public void AddParameter(String name, String value)
        {
            this.request.AddParameter(name, value);
        }

        public void AddFile(string path)
        {
            string mimeType = MimeMapping.GetMimeMapping(path);
            request.AddFile("file", path, "application/octet-stream");
        }


        public async Task<IRestResponse> DoGet()
        {
            this.request.Method = Method.GET;

            return await client.ExecuteTaskAsync(this.request);
        }

        public async Task<IRestResponse> DoPost(object data)
        {
            this.request.Method = Method.POST;
            this.request.AddHeader("Content-type", "application/json");
            this.request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(data);

            return await client.ExecuteTaskAsync(this.request);
        }

        public async Task<IRestResponse> DoPost()
        {
            this.request.Method = Method.POST;
            this.request.AlwaysMultipartFormData = true;

            return await client.ExecuteTaskAsync(this.request);
        }

        public async Task<IRestResponse> DoPut(object data)
        {
            this.request.Method = Method.PUT;
            this.request.AddHeader("Content-type", "application/json");
            this.request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(data);

            return await client.ExecuteTaskAsync(this.request);
        }

        public async Task<IRestResponse> DoDelete()
        {
            this.request.Method = Method.DELETE;

            return await client.ExecuteTaskAsync(this.request);
        }
    }
}