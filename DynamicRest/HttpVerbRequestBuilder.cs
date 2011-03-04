using System;
using System.Net;

using DynamicRest.Helpers;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest
{
    public class HttpVerbRequestBuilder : IBuildRequests
    {
        private readonly IHttpRequestFactory _requestFactory;
        private readonly WebHeaderCollection _headers = new WebHeaderCollection();
        private string _acceptHeader;

        public HttpVerbRequestBuilder(IHttpRequestFactory requestFactory){
            _requestFactory = requestFactory;

            ParametersStore = new ParametersStore();
        }

        public ParametersStore ParametersStore { get; set; }
        public string Body { get; set; }
        public string ContentType { get; set; }
        public ICredentials Credentials { private get; set; }
        public string Uri { private get; set; }

        public void AddHeader(HttpRequestHeader headerType, string value){
            _headers.Add(headerType, value);
        }

        public IHttpRequest CreateRequest(string operationName, JsonObject parameters){
            if (string.IsNullOrEmpty(Uri)){
                throw new InvalidOperationException("You must set a Uri for the request.");
            }

            return CreateWebRequest(operationName);
        }

        public void SetAcceptHeader(string value){
            this._acceptHeader = value;
        }

        private IHttpRequest CreateWebRequest(string operationName){
            var webRequest = _requestFactory.Create(new Uri(Uri));
            webRequest.HttpVerb = operationName.ToHttpVerb();
            webRequest.AddHeaders(_headers);

            webRequest.AddCredentials(Credentials);
            webRequest.Accept = _acceptHeader;

            webRequest.AddRequestBody(ContentType, Body);

            return webRequest;
        }
    }
}
