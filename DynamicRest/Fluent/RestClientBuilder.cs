namespace DynamicRest.Fluent {
    public class RestClientBuilder : IRestClientBuilder {
        IBuildRequests _requestBuilder;
        string _uri;
        string _contentType;
        string _body;
        string _acceptType;
        string _token;

        public dynamic Build() {
            _requestBuilder.Uri = _uri;
            _requestBuilder.ContentType = _contentType;
            _requestBuilder.Body = _body;
            _requestBuilder.AcceptHeader = _acceptType;
            _requestBuilder.SetOAuth2AuthorizationHeader(_token);
            return new RestClient(_requestBuilder, new ResponseProcessor(RestService.Xml, new StandardResultBuilder()));
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

        public IRestClientBuilder WithOAuth2Token(string token) {
            _token = token;
            return this;
        }
    }
}