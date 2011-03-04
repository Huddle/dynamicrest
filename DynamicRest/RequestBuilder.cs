using System;
using System.Net;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest
{
    public class RequestBuilder : IBuildRequests
    {
        private Uri _requestUri;
        private readonly IHttpRequestFactory _requestFactory;

        private readonly IBuildUris _uriBuilder;

        private readonly WebHeaderCollection _headers = new WebHeaderCollection();

        private string _acceptHeader;

        public ParametersStore ParametersStore { get; set; }

        public RequestBuilder(Uri requestUri, IHttpRequestFactory requestFactory, IBuildUris uriBuilder)
        {
            _requestUri = requestUri;
            _requestFactory = requestFactory;
            _uriBuilder = uriBuilder;

            ParametersStore = new ParametersStore();
        }

        public string Body { get; set; }
        public string ContentType { get; set; }
        public ICredentials Credentials { private get; set; }

        public void AddHeader(HttpRequestHeader headerType, string value)
        {
            _headers.Add(headerType, value);
        }

        public IHttpRequest CreateRequest(string operationName, JsonObject parameters)
        {
            _requestUri = BuildUri(operationName, parameters);
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

        private Uri BuildUri(string operationName, JsonObject parameters)
        {
            if (_uriBuilder == null)
            {
                throw new InvalidOperationException("You ust set a template builder before trying to build the Uri");
            }

            if (_requestUri == null)
            {
                if (_uriBuilder is TemplatedUriBuilder)
                {
                    ((TemplatedUriBuilder)_uriBuilder).ParametersStore = ParametersStore;
                }

                _requestUri = _uriBuilder.CreateRequestUri(operationName, parameters);
            }

            return _requestUri;
        }

    }
}
