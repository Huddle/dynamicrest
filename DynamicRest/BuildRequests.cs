using System;
using System.Net;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest
{
    public class BuildRequests : IBuildRequests
    {
        private Uri _requestUri;
        private TemplatedUriBuilder _templateUriBuilder;
        private readonly IHttpRequestFactory _requestFactory;
        private readonly WebHeaderCollection _headers = new WebHeaderCollection();

        public BuildRequests(Uri requestUri, IHttpRequestFactory requestFactory)
        {
            _requestUri = requestUri;
            _requestFactory = requestFactory;
        }

        public string Body { get; set; }
        public string ContentType { get; set; }
        public ICredentials Credentials { private get; set; }

        public string UriTemplate
        {
            get { return _templateUriBuilder.UriTemplate; }
            set { _templateUriBuilder.UriTemplate = value;}
        }

        public void AddHeader(HttpRequestHeader headerType, string value)
        {
            _headers.Add(headerType, value);
        }

        public IHttpRequest CreateRequest(string operationName, JsonObject parameters)
        {
            BuildUri(operationName, parameters);

            return CreateWebRequest();
        }

        public void SetUriBuilder(TemplatedUriBuilder templateUriBuilder )
        {
            _templateUriBuilder = templateUriBuilder;
        }

        private void BuildUri(string operationName, JsonObject parameters)
        {
            if (_templateUriBuilder == null){
                throw new InvalidOperationException("You ust set a template builder before trying to build the Uri");
            }

            if (_requestUri == null){
                _requestUri = _templateUriBuilder.CreateRequestUri(operationName, parameters);
            }
        }

        private IHttpRequest CreateWebRequest()
        {
            var webRequest = _requestFactory.Create(_requestUri);
            //TODO: replace with a strategy approach
            //webRequest.SetHttpVerb((HttpVerb)Enum.Parse(typeof(HttpVerb), operationName));
            webRequest.AddHeaders(_headers);

            webRequest.AddCredentials(Credentials);
 
            webRequest.AddRequestBody(ContentType, Body);
 
            return webRequest;
        }

    }
}
