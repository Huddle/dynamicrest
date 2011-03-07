using System;
using System.Net;

using DynamicRest.UnitTests.TestDoubles;
using Machine.Specifications;

namespace DynamicRest.UnitTests.Fluent {
    public class When_a_rest_client_is_created_with_a_fluent_factory {

        static IRestClientBuilder _restClientBuilder;
        static dynamic _builtClient;

        static FakeHttpRequestFactory fakeHttpRequestFactory;

        Establish context = () => {
            _restClientBuilder = new RestClientBuilder();
        };

        Because the_rest_client_is_built_and_executed = () => {
            fakeHttpRequestFactory = new FakeHttpRequestFactory();
            _builtClient = _restClientBuilder
                .WithRequestBuilder(new HttpVerbRequestBuilder(fakeHttpRequestFactory))
                .WithOAuth2Token("token")
                .WithContentType("application/vnd.data+xml")
                .WithUri("http://www.google.com")
                .WithBody("My body")
                .WithAcceptHeader("application/xml") 
                .Build();

            _builtClient.Post();
        };

        It should_have_the_correct_content_type_header = () =>
            (fakeHttpRequestFactory.CreatedRequest as FakeHttpWebRequestWrapper).GetContentType().ShouldEqual("application/vnd.data+xml");

        It should_have_the_correct_uri = () => 
            fakeHttpRequestFactory.CreatedRequest.RequestURI.ShouldEqual(new Uri("http://www.google.com"));

        It should_have_the_correct_body = () =>
            (fakeHttpRequestFactory.CreatedRequest as FakeHttpWebRequestWrapper).GetRequestBody().ShouldEqual("My body");

        It should_have_the_correct_accept_header =
            () => fakeHttpRequestFactory.CreatedRequest.Accept.ShouldEqual("application/xml");

        It should_have_the_correct_token =
            () => fakeHttpRequestFactory.CreatedRequest.Headers[HttpRequestHeader.Authorization].ShouldEqual("OAuth2 token");
    }

    public interface IRestClientBuilder {
        dynamic Build();

        IRestClientBuilder WithContentType(string contentType);
        IRestClientBuilder WithRequestBuilder(IBuildRequests requestBuilder);
        IRestClientBuilder WithUri(string uri);
        IRestClientBuilder WithBody(string body);
        IRestClientBuilder WithAcceptHeader(string acceptType);
        IRestClientBuilder WithOAuth2Token(string token);
    }

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
            _requestBuilder.SetOAuth2AuthorizationHeader("token");
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