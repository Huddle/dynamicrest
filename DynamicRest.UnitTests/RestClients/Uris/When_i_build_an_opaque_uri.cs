using System;

using DynamicRest.UnitTests.TestDoubles;

using Machine.Specifications;

namespace DynamicRest.UnitTests.RestClients.Uris
{
    [Subject("Opaque uri building")]
    public class When_i_build_an_opaque_uri
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
    }
}