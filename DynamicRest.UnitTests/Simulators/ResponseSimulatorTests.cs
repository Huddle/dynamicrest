using System.Net;
using System.Runtime.Serialization;
using DynamicRest.Fluent;
using DynamicRest.UnitTests.TestDoubles;
using Machine.Specifications;

namespace DynamicRest.UnitTests.Simulators
{
    [Subject(typeof(RestClientBuilder))]
    public class When_a_rest_client_is_created_with_a_rest_client_builder_passing_in_an_expected_response
    {
        static IRestClientBuilder restClientBuilder;
        private static RestOperation _response;
        static dynamic builtClient;

        static FakeHttpRequestFactory fakeHttpRequestFactory;

        Establish context = () =>
        {
            restClientBuilder = new RestClientBuilder();
        };

        Because the_rest_client_is_built_and_executed = () =>
        {
            fakeHttpRequestFactory = new FakeHttpRequestFactory();
            var responseContent = new FakeHttpWebResponseWrapper.ResponseContent("application/vnd.data+xml");
            responseContent.SetContent("<results><message>Test</message></results>");
            fakeHttpRequestFactory.SetResponse(new FakeHttpWebResponseWrapper
                                  {
                                      Headers = new WebHeaderCollection(),
                                      StatusCode = HttpStatusCode.OK,
                                      Content = responseContent

                                  });

            _response = restClientBuilder
                .WithRequestBuilder(new HttpVerbRequestBuilder(fakeHttpRequestFactory))
                .WithUri("http://www.google.com")
                .Build()
                .Get();
        };

        It should_return_a_value_built_from_interpreting_the_response = () =>ShouldExtensionMethods.ShouldEqual(_response.Result.message.Value, "Test");

    }


    [Subject(typeof(RestClientBuilder))]
    public class When_a_rest_client_is_created_with_a_rest_client_builder_passing_in_a_templated_response
    {
        static IRestClientBuilder restClientBuilder;
        private static RestOperation _response;
        static dynamic builtClient;

        static FakeHttpRequestFactory fakeHttpRequestFactory;

        Establish context = () =>
        {
            restClientBuilder = new RestClientBuilder();
        };

        Because the_rest_client_is_built_and_executed = () =>
        {
            fakeHttpRequestFactory = new FakeHttpRequestFactory();
            var responseContent = new FakeHttpWebResponseWrapper.ResponseContent("application/vnd.data+xml");
            responseContent.SetContent(new Result { Message = "Test" });
            fakeHttpRequestFactory.SetResponse(new FakeHttpWebResponseWrapper
                                  {
                                      Headers = new WebHeaderCollection(),
                                      StatusCode = HttpStatusCode.OK,
                                      Content = responseContent

                                  });

            _response = restClientBuilder
                .WithRequestBuilder(new HttpVerbRequestBuilder(fakeHttpRequestFactory))
                .WithUri("http://www.google.com")
                .Build()
                .Get();
        };

        It should_return_a_value_built_from_interpreting_the_response = () => ShouldExtensionMethods.ShouldEqual(_response.Result.Message.Value, "Test");

    }
    public class Result
    {
        public string Message { get; set; }
    }

}
