using System;
using System.Net;
using DynamicRest.Fluent;
using DynamicRest.UnitTests.TestDoubles;
using Machine.Specifications;

namespace DynamicRest.UnitTests.Fluent {
    [Subject(typeof(RestClientBuilder))]
    public class When_a_rest_client_is_created_with_a_rest_client_builder_using_default_values {
        static IRestClientBuilder restClientBuilder;
        static dynamic builtClient;

        static FakeHttpRequestFactory fakeHttpRequestFactory;

        Establish context = () =>
        {
            restClientBuilder = new RestClientBuilder();
        };

        Because the_rest_client_is_built_and_executed = () =>
        {
            fakeHttpRequestFactory = new FakeHttpRequestFactory();
            builtClient = restClientBuilder
                .WithRequestBuilder(new HttpVerbRequestBuilder(fakeHttpRequestFactory))
                .WithOAuth2Token("token")
                .WithUri("http://www.google.com")
                .WithBody("My body")
                .Build();

            builtClient.Post();
        };

        It should_have_xml_as_content_type = () => 
            fakeHttpRequestFactory.CreatedRequest.ContentType.ShouldEqual("application/xml");
        It should_have_xml_as_accept = () => 
            fakeHttpRequestFactory.CreatedRequest.Accept.ShouldEqual("application/xml");
        It should_have_the_allow_auto_redirect_flag_set_to_false = () => 
            fakeHttpRequestFactory.CreatedRequest.AllowAutoRedirect.ShouldEqual(false);
    }

    [Subject(typeof(RestClientBuilder))]
    public class When_a_rest_client_is_created_with_a_rest_client_builder {

        static IRestClientBuilder restClientBuilder;
        static dynamic builtClient;

        static FakeHttpRequestFactory fakeHttpRequestFactory;

        Establish context = () => {
            restClientBuilder = new RestClientBuilder();
        };

        Because the_rest_client_is_built_and_executed = () => {
            fakeHttpRequestFactory = new FakeHttpRequestFactory();
            builtClient = restClientBuilder
                .WithRequestBuilder(new HttpVerbRequestBuilder(fakeHttpRequestFactory))
                .WithOAuth2Token("token")
                .WithContentType("application/vnd.data+xml")
                .WithUri("http://www.google.com")
                .WithBody("My body")
                .WithAcceptHeader("application/xml") 
                .WithResponseProcessor(new ResponseProcessor(new StandardResultBuilder(RestService.Xml)))
                .WithAutoRedirect(true)
                .WithAcceptEncodingHeader("gzip")
                .Build();

            builtClient.Post();
        };

        It should_have_the_correct_content_type_header = () =>
            fakeHttpRequestFactory.CreatedRequest.ContentType.ShouldEqual("application/vnd.data+xml");
        It should_have_the_correct_uri = () => 
            fakeHttpRequestFactory.CreatedRequest.RequestURI.ShouldEqual(new Uri("http://www.google.com"));
        It should_have_the_correct_body = () =>
            fakeHttpRequestFactory.CreatedRequest.RequestBody.ShouldEqual("My body");
        It should_have_the_correct_accept_header = () => 
            fakeHttpRequestFactory.CreatedRequest.Accept.ShouldEqual("application/xml");
        It should_have_the_correct_token = () => 
            fakeHttpRequestFactory.CreatedRequest.Headers[HttpRequestHeader.Authorization].ShouldEqual("OAuth2 token");
        It should_have_the_allow_auto_redirect_flag_set_to_false = () =>
            fakeHttpRequestFactory.CreatedRequest.AllowAutoRedirect.ShouldEqual(true);
        It should_have_the_accept_encode_header = () =>
            fakeHttpRequestFactory.CreatedRequest.Headers[HttpRequestHeader.AcceptEncoding].ShouldEqual("gzip");
    }
}