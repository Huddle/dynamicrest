using System;
using System.Net;

namespace DynamicRest.HTTPInterfaces
{
    public interface IHttpRequest
    {
        Uri RequestURI { get; }
        HttpVerb HttpVerb { get; set; }
        string Accept { get; set; }
        WebHeaderCollection Headers { get; }
        void AddCredentials(ICredentials credentials);
        void AddHeaders(WebHeaderCollection headers);
        void AddRequestBody(string contentType, string content);
        void BeginGetResponse(Action<object> action, object asyncRequest);
        IHttpResponse EndGetResponse(object asyncRequest);
        IHttpResponse GetResponse();
        void SetContentHeaders(string contentType, int contentLength);
    }
    public enum HttpVerb
    {
        Get,
        Post,
        Put,
        Delete,
        Options,
        Head,
        Patch
    }
}