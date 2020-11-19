using System.IO;
using System.Net;

namespace DynamicRest.HTTPInterfaces.HttpWebRequestWrappers {

    public class HttpWebResponseWrapper : IHttpResponse {

        private readonly HttpWebResponse webResponse;

        public HttpWebResponseWrapper(HttpWebResponse webResponse) {
            this.webResponse = webResponse;
        }

        public string ContentEncoding => webResponse.ContentEncoding;

        public long ContentLength => webResponse.ContentLength;

        public WebHeaderCollection Headers => webResponse.Headers;

        public HttpStatusCode StatusCode => webResponse.StatusCode;

        public string StatusDescription => webResponse.StatusDescription;

        public Stream GetResponseStream() {
            return webResponse.GetResponseStream();
        }
    }
}
