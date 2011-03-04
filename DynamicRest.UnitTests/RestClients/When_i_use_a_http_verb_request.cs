using System;

using DynamicRest.HTTPInterfaces;
using DynamicRest.UnitTests.TestDoubles;

using Machine.Specifications;

namespace DynamicRest.UnitTests.RestClients.Uris
{
    [Subject(typeof(RestClient))]
    public class When_i_use_a_http_verb_request
    {
        private const string testUri = "http://api.huddle.local/v2/tasks/123456";

        private static dynamic _client;
        private static FakeHttpRequestFactory _requestFactory;

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(_requestFactory) { Uri = testUri };
            _client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(RestService.Xml));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => _client.Post();

        It should_build_the_expected_uri = () => testUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());

        It should_set_the_correct_http_verb_on_the_request = () => _requestFactory.CreatedRequest.HttpVerb.ShouldEqual(HttpVerb.Post);
    }

    [Subject(typeof(RestClient))]
    public class When_i_set_an_xml_body_on_the_request
    {
        private const string _contentType = @"application\xml+";
        private static FakeHttpRequestFactory _requestFactory;
        private static dynamic _client;
        private static string _requestBody = "<document title='My document title'></document>";

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();

            var httpVerbRequestBuilder = new HttpVerbRequestBuilder(_requestFactory) { Body = _requestBody, ContentType = _contentType, Uri = "http://api.huddle.com" };
            _client = new RestClient(httpVerbRequestBuilder, new ResponseProcessor(RestService.Xml));
        };

        Because we_set_an_xml_body_on_the_test = () => _client.Post();

        It should_set_the_body_of_the_request = () => ((FakeHttpWebRequestWrapper)_requestFactory.CreatedRequest).Body.ShouldEqual(_requestBody);

        It should_set_the_content_type_of_the_request = () => ((FakeHttpWebRequestWrapper)_requestFactory.CreatedRequest).ContentType.ShouldEqual(_contentType);

    }
}