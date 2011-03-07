using System.IO;
using System.Net;

namespace DynamicRest.HTTPInterfaces
{
    public class HttpWebResponseWrapper : IHttpResponse
    {
        private readonly HttpWebResponse webResponse;

        public HttpWebResponseWrapper(HttpWebResponse webResponse)
        {
            this.webResponse = webResponse;
        }

        public WebHeaderCollection Headers
        {
            get
            {
                return webResponse.Headers;
            }
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return webResponse.StatusCode;
            }
        }

        public string StatusDescription
        {
            get
            {
                return webResponse.StatusDescription;
            }
        }

        public Stream GetResponseStream()
        {
            return webResponse.GetResponseStream();
        }
    }
}