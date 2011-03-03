using System;
using System.Net;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest
{
    public class RequestBuilder : IBuildRequests
    {
        private Uri _requestUri;
        private readonly IHttpRequestFactory _requestFactory;
        private readonly WebHeaderCollection _headers = new WebHeaderCollection();

        private string _acceptHeader;

        public RequestBuilder(Uri requestUri, IHttpRequestFactory requestFactory)
        {
            _requestUri = requestUri;
            _requestFactory = requestFactory;
        }

        public string Body { get; set; }
        public string ContentType { get; set; }
        public ICredentials Credentials { private get; set; }

        public void AddHeader(HttpRequestHeader headerType, string value)
        {
            _headers.Add(headerType, value);
        }

        public IHttpRequest CreateRequest(Uri uri, string operationName, JsonObject parameters)
        {
            _requestUri = uri;
            return CreateWebRequest();
        }

        public void SetAcceptHeader(string value)
        {
            this._acceptHeader = value;
        }

        private IHttpRequest CreateWebRequest()
        {
            var webRequest = _requestFactory.Create(_requestUri);
            //TODO: replace with a strategy approach
            //webRequest.SetHttpVerb((HttpVerb)Enum.Parse(typeof(HttpVerb), operationName));
            webRequest.AddHeaders(_headers);

            webRequest.AddCredentials(Credentials);
            webRequest.Accept = _acceptHeader;
 
            webRequest.AddRequestBody(ContentType, Body);
 
            return webRequest;
        }

    }
}
