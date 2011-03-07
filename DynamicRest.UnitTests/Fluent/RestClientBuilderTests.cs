using System;
using System.Net;

using DynamicRest.Fluent;
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
}