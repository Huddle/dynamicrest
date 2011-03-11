using System.IO;
using System.Net;

namespace DynamicRest.HTTPInterfaces {

    public interface IHttpResponse {
        WebHeaderCollection Headers { get; }
        HttpStatusCode StatusCode { get; }
        string StatusDescription { get; }
        Stream GetResponseStream();
    }
}