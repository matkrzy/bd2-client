using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;
using RestSharp.Serializers;

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
        private String JWT = "";
        private AuthorizationType authType = AuthorizationType.Cookies;

        private RestClient client = null;
        private RestRequest request = null;

        private void AddJwtToken()
        {
            if (!JWT.Equals(""))
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
        }

        public Request(String endpoint)
        {
            this.JWT = ConfigurationManager.AppSettings["JWT"];
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

        public void addJsonBody(object body)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(body, settings),
                ParameterType.RequestBody);
        }

        public async Task<IRestResponse> DoGet()
        {
            this.request.Method = Method.GET;

            return await ExecuteRequest();
        }

        public async Task<IRestResponse> DoPost(object data)
        {
            this.request.Method = Method.POST;
            this.request.AddHeader("Content-type", "application/json");
            this.request.RequestFormat = DataFormat.Json;
//            request.AddJsonBody(data);
            this.addJsonBody(data);

            return await ExecuteRequest();
        }

        public async Task<IRestResponse> DoPost()
        {
            this.request.Method = Method.POST;
            this.request.AlwaysMultipartFormData = true;

            return await ExecuteRequest();
        }

        public async Task<IRestResponse> DoPut(object data)
        {
            this.request.Method = Method.PUT;
            this.request.AddHeader("Content-type", "application/json");
            this.request.RequestFormat = DataFormat.Json;
//            var output = JsonConvert.SerializeObject(data);
//            request.AddBody(output);
//
//            request.AddParameter("application/json", JsonConvert.SerializeObject(data), ParameterType.RequestBody);

            this.addJsonBody(data);

            return await ExecuteRequest();
        }

        public async Task<IRestResponse> DoDelete()
        {
            this.request.Method = Method.DELETE;

            return await ExecuteRequest();
        }

        public async Task<bool> Download(String path, String name, String extension)
        {
            try
            {
                await Task.Run(() => client.DownloadData(request).SaveAs($"{path}/{name}{extension}"));

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private async Task<IRestResponse> ExecuteRequest()
        {
            try
            {
                return await client.ExecuteTaskAsync(this.request);
            }
            catch (Exception e)
            {
                RestResponse failResponse = new RestResponse();
                failResponse.StatusCode = HttpStatusCode.BadRequest;
                failResponse.Content = "";
                return failResponse;
            }
        }
    }
}