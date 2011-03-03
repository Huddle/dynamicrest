using System;
using System.Net;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest
{
    public interface IBuildRequests
    {
        string Body { get; set; }
        ICredentials Credentials { set; }
        string ContentType { get; set; }
        void AddHeader(HttpRequestHeader headerType, string value);
        IHttpRequest CreateRequest(Uri uri, string operationName, JsonObject parameters);
    }
}