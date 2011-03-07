using System;
using System.Net;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest
{
    public class TemplatedUriRequestBuilder : IBuildRequests
    {
        private readonly IHttpRequestFactory _requestFactory;
        private readonly TemplatedUriBuilder _uriBuilder = new TemplatedUriBuilder();
        private readonly WebHeaderCollection _headers = new WebHeaderCollection();

        public TemplatedUriRequestBuilder(IHttpRequestFactory requestFactory)
        {
            _requestFactory = requestFactory;

            ParametersStore = new ParametersStore();
        }

        public ParametersStore ParametersStore { get; set; }
        public string Body { get; set; }
        public string ContentType { get; set; }
        public ICredentials Credentials { private get; set; }
        public string Uri { private get; set; }
        public string AcceptHeader { get; set; }

        public void AddHeader(HttpRequestHeader headerType, string value)
        {
            _headers.Add(headerType, value);
        }

        public IHttpRequest CreateRequest(string operationName, JsonObject parameters)
        {
            var uri = BuildUri(operationName, parameters);
            
            var webRequest = _requestFactory.Create(uri);
            
            webRequest.AddHeaders(_headers);
            webRequest.AddCredentials(Credentials);
            webRequest.Accept = AcceptHeader;
            webRequest.AddRequestBody(ContentType, Body);
 
            return webRequest;
        }

        private Uri BuildUri(string operationName, JsonObject parameters)
        {
           _uriBuilder.ParametersStore = ParametersStore;
            _uriBuilder.UriTemplate = this.Uri;
            return _uriBuilder.CreateRequestUri(operationName, parameters);
        }

    }
}
