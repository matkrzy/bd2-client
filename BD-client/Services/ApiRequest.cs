using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BD_client.Services
{
    public class ApiRequest
    {
        private static String apiHost = ConfigurationManager.AppSettings["ApiHost"];
        private static String apiPath = ConfigurationManager.AppSettings["ApiPath"];
        private static String apiUrl = apiHost + apiPath;
        public static String JWT = null;

        private static String GetEndpointUrl(String endpointPath)
        {
            return apiUrl + endpointPath;
        }

        //TODO: zmienić te akcje
        public static void Post(String endpointPath, String value)
        {
            byte[] data = null;
            if (value != null)
            {
                data = Encoding.ASCII.GetBytes(value);
            }

            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(GetEndpointUrl(endpointPath));
            var cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            if (JWT != null)
            {
                Cookie cookie = new Cookie();
                cookie.Name = "JWT";
                cookie.Value = JWT;
                cookie.Domain = request.RequestUri.Host;
                request.CookieContainer.Add(cookie);
            }

            request.Method = "POST";
            if (value != null)
            {
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }

            var response = (HttpWebResponse) request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created)
            {
                throw new Exception();
            }

            if (JWT == null)
            {
                var allCookies = new CookieCollection();
                var domainTableField = cookieContainer.GetType().GetRuntimeFields()
                    .FirstOrDefault(x => x.Name == "m_domainTable");
                var domains = (IDictionary) domainTableField.GetValue(cookieContainer);

                foreach (var val in domains.Values)
                {
                    var type = val.GetType().GetRuntimeFields().First(x => x.Name == "m_list");
                    var values = (IDictionary) type.GetValue(val);
                    foreach (CookieCollection cookies in values.Values)
                    {
                        allCookies.Add(cookies);
                    }
                }

                CookieCollection responseCookies = allCookies;

                if (responseCookies.Count > 0)
                {
                    JWT = responseCookies[0].Value;
                    String name = responseCookies[0].Name;
                }
            }
        }

        #region Piotrek
        public static async Task<HttpResponseMessage> GetAsync(string endpointPath)
        {
            var baseUri = new Uri(apiHost);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler {CookieContainer = cookieContainer})
            using (var client = new HttpClient(handler) {BaseAddress = baseUri})
            {
                cookieContainer.Add(baseUri, new Cookie("JWT", JWT));
                return await client.GetAsync(GetEndpointUrl(endpointPath));
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(string endpointPath, object content)
        {
            var cookieContainer = new CookieContainer();
            var baseUri = new Uri(apiHost);
            using (var handler = new HttpClientHandler {CookieContainer = cookieContainer})
            using (var client = new HttpClient(handler) {BaseAddress = baseUri})
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8,
                    "application/json");
                cookieContainer.Add(baseUri, new Cookie("JWT", JWT));
                return await client.PostAsync(GetEndpointUrl(endpointPath), stringContent);
            }
        }

        public static async Task<HttpResponseMessage> DeleteAsync(string endpointPath, object content = null)
        {
            var cookieContainer = new CookieContainer();
            var baseUri = new Uri(apiHost);
            using (var handler = new HttpClientHandler {CookieContainer = cookieContainer})
            using (var client = new HttpClient(handler) {BaseAddress = baseUri})
            {
                cookieContainer.Add(baseUri, new Cookie("JWT", JWT));

                if (content == null)
                {
                    return await client.DeleteAsync(GetEndpointUrl(endpointPath));
                }
                else
                {
                    var request = new HttpRequestMessage
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8,
                            "application/json"),
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(GetEndpointUrl(endpointPath))
                    };
                    return await client.SendAsync(request);
                }
            }
        }

        public static async Task<HttpResponseMessage> PutAsync(string endpointPath, object content)
        {
            var cookieContainer = new CookieContainer();
            var baseUri = new Uri(apiHost);
            using (var handler = new HttpClientHandler {CookieContainer = cookieContainer})
            using (var client = new HttpClient(handler) {BaseAddress = baseUri})
            {
                var stringContent = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8,
                    "application/json");
                cookieContainer.Add(baseUri, new Cookie("JWT", JWT));
                return await client.PutAsync(GetEndpointUrl(endpointPath), stringContent);
            }
        }
        #endregion

        public static async Task<bool> PostFile(string endpointPath, string pathToFile, string fileName)
        {
            var reader = File.Open(pathToFile, FileMode.Open);
            var fileStreamContent = new StreamContent(reader);
            var baseUri = new Uri(apiHost);
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() {CookieContainer = cookieContainer})
            using (var formData = new MultipartFormDataContent())
            using (var client = new HttpClient(handler) {BaseAddress = baseUri})
            {
                cookieContainer.Add(baseUri, new Cookie("JWT", JWT));
                formData.Add(fileStreamContent, "file", fileName);
                var response = await client.PostAsync(GetEndpointUrl(endpointPath), formData);
                return response.IsSuccessStatusCode;
            }
        }

        public static void Put(String endpointPath, String value)
        {
            var bytes = Encoding.ASCII.GetBytes(value);
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(GetEndpointUrl(endpointPath));
            var cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            if (JWT != null)
            {
                Cookie cookie = new Cookie();
                cookie.Name = "JWT";
                cookie.Value = JWT;
                cookie.Domain = request.RequestUri.Host;
                request.CookieContainer.Add(cookie);
            }

            request.Method = "PUT";
            request.ContentType = "application/json";
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }

            var response = (HttpWebResponse) request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }

        public static void PutInParams(String endpointPath)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(GetEndpointUrl(endpointPath));
            var cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            if (JWT != null)
            {
                Cookie cookie = new Cookie();
                cookie.Name = "JWT";
                cookie.Value = JWT;
                cookie.Domain = request.RequestUri.Host;
                request.CookieContainer.Add(cookie);
            }

            request.Method = "PUT";

            var response = (HttpWebResponse) request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }

        public static String Get(String endpointPath)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(GetEndpointUrl(endpointPath));
            var cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            if (JWT != null)
            {
                Cookie cookie = new Cookie();
                cookie.Name = "JWT";
                cookie.Value = JWT;
                cookie.Domain = request.RequestUri.Host;
                request.CookieContainer.Add(cookie);
            }

            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            string responseContent = null;

            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader sr99 = new StreamReader(stream))
                    {
                        responseContent = sr99.ReadToEnd();
                    }
                }
            }

            return responseContent;
        }

        public static void Delete(String endpointPath)
        {
            HttpWebRequest request = (HttpWebRequest) HttpWebRequest.Create(GetEndpointUrl(endpointPath));
            var cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            if (JWT != null)
            {
                Cookie cookie = new Cookie();
                cookie.Name = "JWT";
                cookie.Value = JWT;
                cookie.Domain = request.RequestUri.Host;
                request.CookieContainer.Add(cookie);
            }

            request.Method = "DELETE";

            var response = (HttpWebResponse) request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception();
            }
        }
    }
}