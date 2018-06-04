using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        private static String JWT = ConfigurationManager.AppSettings["JWT"];
        private static String apiUrl = apiHost + apiPath;
        private HttpWebRequest request = null;
        private AuthorizationType authType = AuthorizationType.Cookies;

        private void AddJwtToken()
        {
            switch (this.authType)
            {
                case AuthorizationType.Cookies:
                {
                    Cookie cookie = new Cookie
                    {
                        Name = "JWT",
                        Value = JWT,
                        Domain = request.RequestUri.Host
                    };

                    this.request.CookieContainer = new CookieContainer();
                    this.request.CookieContainer.Add(cookie);
                    break;
                }
                case AuthorizationType.Header:
                {
                    this.request.Headers["Authorization"] = "Bearer " + JWT;
                    break;
                }
            }
        }

        private void addBody(object data)
        {
            string dataString = JsonConvert.SerializeObject(data, Formatting.Indented);

            request.ContentLength = dataString.Length;
            using (Stream stream = this.request.GetRequestStream())
            {
                stream.Write(Encoding.ASCII.GetBytes(dataString), 0, dataString.Length);
            }
        }

        public Request(String endpoint)
        {
            String requestUrl = apiUrl + endpoint;
            this.request = (HttpWebRequest) WebRequest.Create(requestUrl);
            this.AddJwtToken();
        }


        public Response DoPost(object data)
        {
            this.request.Method = "POST";
            this.request.ContentType = "application/json";

            this.addBody(data);

            return new Response(this.request);
        }

        public Response DoPost(MultipartFormDataContent form)
        {
            this.request.Method = "POST";
            this.request.ContentType = "application/json";

            //TODO add multipart form body convert

            return new Response(this.request);
        }

        public Response DoGet(object data)
        {
            this.request.Method = "GET";

            return new Response(this.request);
        }

        public Response DoGet()
        {
            this.request.Method = "GET";

            return new Response(this.request);
        }

        public Response DoPut(object data)
        {
            this.request.Method = "PUT";
            this.request.ContentType = "application/json";

            this.addBody(data);

            return new Response(this.request);
        }

        public Response DoDelete()
        {
            this.request.Method = "DELETE";

            return new Response(this.request);
        }
    }
}