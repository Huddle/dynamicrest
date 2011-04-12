using System;
using System.Net;
using DynamicRest.Helpers;
using DynamicRest.HTTPInterfaces;
using DynamicRest.Json;

namespace DynamicRest {

    public class HttpVerbRequestBuilder : IBuildRequests {

        private readonly IHttpRequestFactory _requestFactory;
        private readonly WebHeaderCollection _headers = new WebHeaderCollection();

        public HttpVerbRequestBuilder(IHttpRequestFactory requestFactory) {
            _requestFactory = requestFactory;

            ParametersStore = new ParametersStore();
        }

        public ParametersStore ParametersStore { get; set; }
        public string Body { get; set; }
        public string ContentType { get; set; }
        public ICredentials Credentials { private get; set; }
        public string Uri { private get; set; }
        public string AcceptHeader { get; set; }
        public bool AllowAutoRedirect { get; set; }

        public IHttpRequest CreateRequest(string operationName, JsonObject parameters) {
            if (string.IsNullOrEmpty(Uri)) {
                throw new InvalidOperationException("You must set a Uri for the request.");
            }
            var webRequest = _requestFactory.Create(new Uri(Uri));
            
            webRequest.HttpVerb = operationName.ToHttpVerb();
            webRequest.AddHeaders(_headers);
            webRequest.AddCredentials(Credentials);
            webRequest.Accept = AcceptHeader;
            webRequest.AddRequestBody(ContentType, Body);
            webRequest.AllowAutoRedirect = AllowAutoRedirect;

            return webRequest;
        }

        public void SetOAuth2AuthorizationHeader(string oAuth2Token) {
            _headers.Add(HttpRequestHeader.Authorization, string.Format("OAuth2 {0}", oAuth2Token));
        }

        public void AddHeader(HttpRequestHeader headerType, string value) {
            _headers.Add(headerType, value);
        }
    }
}
