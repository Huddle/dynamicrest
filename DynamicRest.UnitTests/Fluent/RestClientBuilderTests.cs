using System;
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
    }

    public interface IRestClientBuilder {
        dynamic Build();

        IRestClientBuilder WithContentType(string contentType);
        IRestClientBuilder WithRequestBuilder(IBuildRequests requestBuilder);
        IRestClientBuilder WithUri(string uri);
        IRestClientBuilder WithBody(string body);
        IRestClientBuilder WithAcceptHeader(string acceptType);
    }

    public class RestClientBuilder : IRestClientBuilder {
        IBuildRequests _requestBuilder;
        string _uri;
        string _contentType;
        string _body;
        string _acceptType;

        public dynamic Build() {
            _requestBuilder.Uri = _uri;
            _requestBuilder.ContentType = _contentType;
            _requestBuilder.Body = _body;
            _requestBuilder.AcceptHeader = _acceptType;
            return new RestClient(_requestBuilder, new ResponseProcessor(RestService.Xml));
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
    }
}