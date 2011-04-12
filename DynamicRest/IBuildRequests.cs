using System.Net;
using DynamicRest.HTTPInterfaces;
using DynamicRest.Json;

namespace DynamicRest
{
    public interface IBuildRequests {
        string Uri { set; }
        string ContentType { get; set; }
        string AcceptHeader { get; set; }
        string Body { get; set; }
        ParametersStore ParametersStore { get; set; }
        ICredentials Credentials { set; }
        bool AllowAutoRedirect { get; set; }
        IHttpRequest CreateRequest(string operationName, JsonObject parameters);
        void AddHeader(HttpRequestHeader headerType, string value);
        void SetOAuth2AuthorizationHeader(string oAuth2Token);
    }
}