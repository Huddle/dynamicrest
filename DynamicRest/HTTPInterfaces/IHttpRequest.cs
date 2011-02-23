using System;
using System.IO;
using System.Net;

namespace DynamicRest.HTTPInterfaces
{
    public interface IHttpRequest
    {
        Uri RequestURI { get; }
 
        void AddCredentials(ICredentials credentials);
        void AddHeaders(WebHeaderCollection headers);
        void AddRequestBody(string contentType, string content);
        void BeginGetResponse(Action<object> action, object asyncRequest);
        IHttpResponse EndGetResponse(object asyncRequest);
        IHttpResponse GetResponse();
        void SetContentHeaders(string contentType, int contentLength);
        void SetHttpVerb(HttpVerb verb);
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