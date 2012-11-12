using System;
using System.Net;

namespace DynamicRest.HTTPInterfaces
{
    public interface IHttpRequest
    {
        Uri RequestURI { get; }
        HttpVerb HttpVerb { get; set; }
        WebHeaderCollection Headers { get; }
        string Accept { get; set; }
        string ContentType { get; }

        bool AllowAutoRedirect { get; set; }
        string UserAgent { get; set; }

        void AddCredentials(ICredentials credentials);
        void AddHeaders(WebHeaderCollection headers);
        void AddRequestBody(string contentType, string content);
        void BeginGetResponse(Action<object> action, object asyncRequest);
        IHttpResponse EndGetResponse(object asyncRequest);
        IHttpResponse GetResponse();
    }
}