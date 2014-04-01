using System;
using System.Net;
using DynamicRest.HTTPInterfaces;
using DynamicRest.UnitTests.TestDoubles;
using Machine.Specifications;

namespace DynamicRest.UnitTests.RestClients {

    [Subject(typeof(HttpVerbRequestBuilder))]
    public class When_i_use_the_client_to_put_a_request {
        private const string TestUri = "http://api.huddle.local/v2/tasks/123456";
        private static dynamic client;
        private static FakeHttpRequestFactory requestFactory;
        private static string oAuth2Token = "{my_token}";

        Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(requestFactory) { Uri = TestUri };
            httpVerbRequestBuilder.SetOAuth2AuthorizationHeader(oAuth2Token);
            client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => client.Post();

        It should_build_the_expected_uri = () => TestUri.ShouldEqual(requestFactory.CreatedRequest.RequestURI.ToString());
        It should_set_the_correct_http_verb_on_the_request = () => requestFactory.CreatedRequest.HttpVerb.ShouldEqual(HttpVerb.Post);
        It should_set_the_correct_authorization_header_on_the_request = () => requestFactory.CreatedRequest.Headers[HttpRequestHeader.Authorization].ShouldEqual(string.Format("OAuth2 {0}", oAuth2Token));
    }

    [Subject(typeof(HttpVerbRequestBuilder))]
    public class When_i_use_the_client_to_get_a_request {
        private const string TestUri = "http://api.huddle.local/v2/tasks/123456";
        private static dynamic client;
        private static FakeHttpRequestFactory requestFactory;

        Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(requestFactory) { Uri = TestUri };
            client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => client.Get();

        It should_build_the_expected_uri = () => TestUri.ShouldEqual(requestFactory.CreatedRequest.RequestURI.ToString());
        It should_set_the_correct_http_verb_on_the_request = () => requestFactory.CreatedRequest.HttpVerb.ShouldEqual(HttpVerb.Get);
    }

    [Subject(typeof(HttpVerbRequestBuilder))]
    public class When_i_set_an_invalid_operation_on_the_client {
        private const string TestUri = "http://api.huddle.local/v2/tasks/123456";
        private static dynamic client;
        private static dynamic exception;
        private static FakeHttpRequestFactory requestFactory;

        Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(requestFactory) { Uri = TestUri };
            client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => exception = Catch.Exception(() => client.NotHttp());

        It should_throw_an_exception = () => ((Exception) exception).ShouldBeOfType<InvalidOperationException>();
    }

    [Subject(typeof(HttpVerbRequestBuilder))]
    public class When_i_set_an_xml_body_on_the_request {
        private const string ContentType = @"application\xml+";
        private static FakeHttpRequestFactory requestFactory;
        private static dynamic client;
        private static string requestBody = "<document title='My document title'></document>";

        Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(requestFactory) { Body = requestBody, ContentType = ContentType, Uri = "http://api.huddle.com" };
            client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_set_an_xml_body_on_the_test = () => client.Post();

        It should_set_the_body_of_the_request = () => requestFactory.CreatedRequest.RequestBody.ShouldEqual(requestBody);
        It should_set_the_content_type_of_the_request = () => requestFactory.CreatedRequest.ContentType.ShouldEqual(ContentType);
    }

    [Subject(typeof(HttpVerbRequestBuilder))]
    public class When_the_autofollow_option_is_set_to_true {
        private const string TestUri = "http://api.huddle.local/v2/tasks/123456";
        private static dynamic client;
        private static FakeHttpRequestFactory requestFactory;

        Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(requestFactory) { Uri = TestUri, AllowAutoRedirect = true };
            client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => client.Get();

        It should_set_the_auto_follow_option_to_true_on_the_request = () => requestFactory.CreatedRequest.AllowAutoRedirect.ShouldEqual(true);
    }

    [Subject(typeof(HttpVerbRequestBuilder))]
    public class When_the_autofollow_option_is_set_to_false {
        private const string TestUri = "http://api.huddle.local/v2/tasks/123456";
        private static dynamic client;
        private static FakeHttpRequestFactory requestFactory;

        Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(requestFactory) { Uri = TestUri, AllowAutoRedirect = false };
            client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => client.Get();

        It should_set_the_auto_follow_option_to_false_on_the_request = () => requestFactory.CreatedRequest.AllowAutoRedirect.ShouldEqual(false);
    }

    [Subject(typeof(HttpVerbRequestBuilder))]
    public class When_the_autofollow_option_is_not_explicitly_set {
        private const string TestUri = "http://api.huddle.local/v2/tasks/123456";
        private static dynamic client;
        private static FakeHttpRequestFactory requestFactory;

        Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(requestFactory) { Uri = TestUri };
            client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => client.Get();

        It should_default_the_auto_follow_option_to_false_on_the_request = () => requestFactory.CreatedRequest.AllowAutoRedirect.ShouldEqual(false);
    }

    [Subject(typeof(HttpVerbRequestBuilder))]
    public class When_i_use_the_client_to_delete_a_request
    {
        private const string TestUri = "http://api.huddle.local/v2/tasks/123456";
        private static dynamic client;
        private static FakeHttpRequestFactory requestFactory;
        private static string oAuth2Token = "{my_token}";

        Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(requestFactory) { Uri = TestUri };
            httpVerbRequestBuilder.SetOAuth2AuthorizationHeader(oAuth2Token);
            client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => client.Delete();

        It should_build_the_expected_uri = () => TestUri.ShouldEqual(requestFactory.CreatedRequest.RequestURI.ToString());
        It should_set_the_correct_http_verb_on_the_request = () => requestFactory.CreatedRequest.HttpVerb.ShouldEqual(HttpVerb.Delete);
        It should_set_the_correct_authorization_header_on_the_request = () => requestFactory.CreatedRequest.Headers[HttpRequestHeader.Authorization].ShouldEqual(string.Format("OAuth2 {0}", oAuth2Token));
    }

}