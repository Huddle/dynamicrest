using System.IO;
using System.Net;

namespace DynamicRest.HTTPInterfaces {

    public interface IHttpResponse {
        string ContentEncoding { get; }
        long ContentLength { get; }
        WebHeaderCollection Headers { get; }
        HttpStatusCode StatusCode { get; }
        string StatusDescription { get; }
        Stream GetResponseStream();
    }
}