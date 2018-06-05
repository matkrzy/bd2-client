using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BD_client.Api.Core
{
    class Response
    {
        private HttpWebResponse response = null;

        public Response(HttpWebRequest request)
        {
            try
            {
                this.response = (HttpWebResponse) request.GetResponse();
            }
            catch (WebException e)
            {
                this.response = (HttpWebResponse) e.Response;
            }
        }

        public HttpWebResponse AsHttpWebResponse()
        {
            return this.response;
        }

        public String AsString()
        {
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                return reader.ReadToEnd();
            }
        }
    }
}