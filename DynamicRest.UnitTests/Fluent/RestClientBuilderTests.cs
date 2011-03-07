using System;
using DynamicRest.UnitTests.TestDoubles;
using Machine.Specifications;

namespace DynamicRest.UnitTests.Fluent {
    public class When_a_rest_client_is_created_with_a_fluent_factory {

        Establish context = () => {
            _restClientBuilder = new RestClientBuilder();
        };

        Because the_rest_client_is_built_and_executed = () => {
            fakeHttpRequestFactory = new FakeHttpRequestFactory();
            _builtClient = _restClientBuilder
                .WithRequestBuilder(new HttpVerbRequestBuilder(fakeHttpRequestFactory))
                .WithContentType("content/type")
                .WithUri("http://www.google.com")
                .Build();

            _builtClient.Post();
        };

        It should_not_be_null = () => ShouldExtensionMethods.ShouldNotBeNull(_builtClient);

        private It should_have_the_correct_content_type_header = () => 
            (fakeHttpRequestFactory.CreatedRequest as FakeHttpWebRequestWrapper).GetContentType().ShouldEqual("content/type");

        private It should_have_the_correct_uri =
            () => fakeHttpRequestFactory.CreatedRequest.RequestURI.ShouldEqual(new Uri("http://www.google.com"));

        private static IRestClientBuilder _restClientBuilder;
        private static dynamic _builtClient;

        private static FakeHttpRequestFactory fakeHttpRequestFactory;
    }

    public interface IRestClientBuilder {
        dynamic Build();

        IRestClientBuilder WithContentType(string contentType);
        IRestClientBuilder WithRequestBuilder(IBuildRequests requestBuilder);
        IRestClientBuilder WithUri(string uri);
    }

    public class RestClientBuilder : IRestClientBuilder {
        private IBuildRequests _requestBuilder;
        private string _uri;
        private string _contentType;

        public dynamic Build() {
            _requestBuilder.Uri = _uri;
            _requestBuilder.ContentType = _contentType;
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
    }
}