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

        public dynamic Build() {
            _contentType = _contentType ?? "application/xml";
            _acceptType = _acceptType ?? "application/xml";
            if (_noAcceptHeader){
                _acceptType = string.Empty;
            }

            if(_requestBuilder == null) 
                _requestBuilder = new HttpVerbRequestBuilder(new RequestFactory());

            if (_responseProcessor == null) {
                var serviceType = _acceptType.Contains("xml") ? RestService.Xml : (_acceptType.Contains("json") ? RestService.Json : RestService.Binary);
                if (_noAcceptHeader){
                    serviceType = RestService.Xml;
                }

                this._responseProcessor = new ResponseProcessor(new StandardResultBuilder(serviceType));
            }

            _requestBuilder.Uri = _uri;
            _requestBuilder.ContentType = _contentType;
            _requestBuilder.AcceptHeader = _acceptType;
            _requestBuilder.Body = _body;
            _requestBuilder.SetOAuth2AuthorizationHeader(_token);
            _requestBuilder.AllowAutoRedirect = _autoRedirect;
            if(!string.IsNullOrEmpty(_acceptEncodingType))
                _requestBuilder.AddHeader(HttpRequestHeader.AcceptEncoding, _acceptEncodingType);
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
    }
}