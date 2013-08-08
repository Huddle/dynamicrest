using System;
using System.Collections.Generic;
using System.Net;
using DynamicRest.HTTPInterfaces.WebWrappers;

namespace DynamicRest.Fluent {

    public class RestClientBuilder : IRestClientBuilder {

        IBuildRequests _requestBuilder;
        IProcessResponses _responseProcessor;
        string _uri;
        string _contentType;
        string _body;
        string _acceptType;
        string _token;
        bool _noAcceptHeader;
        private bool _autoRedirect;
        private string _acceptEncodingType;
        Dictionary<HttpRequestHeader, string> _headers;
        Dictionary<string, string> _customHeaders;
        DateTime? _ifModifiedSince;
        private string _userAgent;
        private int _timeout;

        IWebProxy _proxy;

        public RestClientBuilder()
        {
            _headers = new Dictionary<HttpRequestHeader, string>();
            _customHeaders = new Dictionary<string, string>();
        }

        public dynamic Build() 
        {
            _timeout = _timeout == 0 ? 100000 : _timeout; // default timeout i the CLR is 100s

            _contentType = _contentType ?? "application/xml";
            _acceptType = _acceptType ?? "application/xml";
            if (_noAcceptHeader){
                _acceptType = string.Empty;
            }

            if (_requestBuilder == null) {
                _requestBuilder = new HttpVerbRequestBuilder(new RequestFactory());
            }

            if (_responseProcessor == null) {
                var serviceType = _acceptType.Contains("xml") ? RestService.Xml : _acceptType.Contains("json") ? RestService.Json : _acceptType.EndsWith("plain") ? RestService.Text: RestService.Binary;
                if (_noAcceptHeader){
                    serviceType = RestService.Xml;
                }

                this._responseProcessor = new ResponseProcessor(new StandardResultBuilder(serviceType));
            }

            if (_proxy != null)
            {
                _requestBuilder.Proxy = _proxy;
            }

            _requestBuilder.Uri = _uri;
            _requestBuilder.ContentType = _contentType;
            _requestBuilder.AcceptHeader = _acceptType;
            _requestBuilder.Body = _body;
            _requestBuilder.AllowAutoRedirect = _autoRedirect;
            _requestBuilder.UserAgent = _userAgent;
            _requestBuilder.Timeout = _timeout;
            if (!string.IsNullOrEmpty(_token))
            {
                _requestBuilder.SetOAuth2AuthorizationHeader(_token);
            }
            if(!string.IsNullOrEmpty(_acceptEncodingType))
            {
                _requestBuilder.AddHeader(HttpRequestHeader.AcceptEncoding, _acceptEncodingType);
            }

            if(_ifModifiedSince.HasValue)
            {

                _requestBuilder.IfModifiedSince(_ifModifiedSince.Value);
            }

            foreach (var header in _headers)
            {
                _requestBuilder.AddHeader(header.Key, header.Value);
            }

            foreach (var header in _customHeaders)
            {
                _requestBuilder.AddCustomHeader(header.Key, header.Value);
            }

            return new RestClient(_requestBuilder, _responseProcessor);
        }

        public IRestClientBuilder WithContentType(string contentType) {
            _contentType = contentType;
            return this;
        }

        public IRestClientBuilder WithRequestBuilder(IBuildRequests requestBuilder) {
            _requestBuilder = requestBuilder;
            return this;
        }

        public IRestClientBuilder WithUri(string uri) {
            _uri = uri;
            return this;
        }

        public IRestClientBuilder WithBody(string body) {
            _body = body;
            return this;
        }

        public IRestClientBuilder WithAcceptHeader(string acceptType) {
            _acceptType = acceptType;
            return this;
        }

        public IRestClientBuilder WithAcceptEncodingHeader(string acceptEncodingType)
        {
            _acceptEncodingType = acceptEncodingType;
            return this;
        }

        public IRestClientBuilder WithIfModifiedSinceDate(DateTime ifModifiedSince)
        {
            _ifModifiedSince = ifModifiedSince;
            return this;
        }

        public IRestClientBuilder WithOAuth2Token(string token) {
            _token = token;
            return this;
        }

        public IRestClientBuilder WithResponseProcessor(IProcessResponses responseProcessor) {
            _responseProcessor = responseProcessor;
            return this;
        }

        public IRestClientBuilder WithNoAcceptHeader() {
            _noAcceptHeader = true;
            return this;
        }

        public IRestClientBuilder WithAutoRedirect(bool autoRedirect) {
            _autoRedirect = autoRedirect;
            return this;
        }

        public IRestClientBuilder WithHeaders(Dictionary<HttpRequestHeader, string> headers)
        {
            foreach (var header in headers)
            {
                _headers.Add(header.Key, header.Value);
            }
            return this;
        }

        public IRestClientBuilder WithCustomHeaders(Dictionary<string, string> headers)
        {
            foreach (var header in headers)
            {
                _customHeaders.Add(header.Key, header.Value);
            }
            return this;
        }

        public IRestClientBuilder WithUserAgent(string userAgent)
        {
            _userAgent = userAgent;
            return this;
        }

        public IRestClientBuilder WithTimeout(int timeout)
        {
            _timeout = timeout;
            return this;
        }

        public IRestClientBuilder WithProxy(IWebProxy webProxy)
        {
            _proxy = webProxy;
            return this;
        }
    }
}