using System;
using System.Net;
using DynamicRest.HTTPInterfaces;
using DynamicRest.UnitTests.TestDoubles;
using Machine.Specifications;

namespace DynamicRest.UnitTests.RestClients {

    [Subject(typeof(HttpVerbRequestBuilder))]
    public class When_i_use_the_client_to_put_a_request {

        private const string TestUri = "http://api.huddle.local/v2/tasks/123456";
        private static dynamic _client;
        private static FakeHttpRequestFactory _requestFactory;
        private static string oAuth2Token = "{my_token}";

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(_requestFactory) { Uri = TestUri };
            httpVerbRequestBuilder.SetOAuth2AuthorizationHeader(oAuth2Token);
            _client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => _client.Post();

        It should_build_the_expected_uri = () => TestUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());
        It should_set_the_correct_http_verb_on_the_request = () => _requestFactory.CreatedRequest.HttpVerb.ShouldEqual(HttpVerb.Post);
        It should_set_the_correct_authorization_header_on_the_request = () => _requestFactory.CreatedRequest.Headers[HttpRequestHeader.Authorization].ShouldEqual(string.Format("OAuth2 {0}", oAuth2Token));
    }

    [Subject(typeof(HttpVerbRequestBuilder))]
    public class When_i_use_the_client_to_get_a_request {

        private const string TestUri = "http://api.huddle.local/v2/tasks/123456";
        private static dynamic _client;
        private static FakeHttpRequestFactory _requestFactory;

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(_requestFactory) { Uri = TestUri };
            _client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => _client.Get();

        It should_build_the_expected_uri = () => TestUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());
        It should_set_the_correct_http_verb_on_the_request = () => _requestFactory.CreatedRequest.HttpVerb.ShouldEqual(HttpVerb.Get);
    }

    [Subject(typeof(HttpVerbRequestBuilder))]
    public class When_i_set_an_invalid_operation_on_the_client {

        private const string testUri = "http://api.huddle.local/v2/tasks/123456";
        private static dynamic _client;
        private static dynamic _exception;
        private static FakeHttpRequestFactory _requestFactory;

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(_requestFactory) { Uri = testUri };
            _client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => _exception = Catch.Exception(() => _client.NotHttp());

        It should_throw_an_exception = () => ((Exception) _exception).ShouldBeOfType<InvalidOperationException>();
    }

    [Subject(typeof(HttpVerbRequestBuilder))]
    public class When_i_set_an_xml_body_on_the_request {

        private const string ContentType = @"application\xml+";
        private static FakeHttpRequestFactory _requestFactory;
        private static dynamic _client;
        private static string _requestBody = "<document title='My document title'></document>";

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(_requestFactory) { Body = _requestBody, ContentType = ContentType, Uri = "http://api.huddle.com" };
            _client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_set_an_xml_body_on_the_test = () => _client.Post();

        It should_set_the_body_of_the_request = () => _requestFactory.CreatedRequest.RequestBody.ShouldEqual(_requestBody);
        It should_set_the_content_type_of_the_request = () => _requestFactory.CreatedRequest.ContentType.ShouldEqual(ContentType);
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
    public class When_the_autofollow_option_is_not_explicitly_set
    {
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
}