using System;

using DynamicRest.HTTPInterfaces.WebWrappers;
using DynamicRest.UnitTests.TestDoubles;
using Machine.Specifications;

namespace DynamicRest.UnitTests.RestClients.Uris
{
    [Subject(typeof(RestClient))]
    public class When_using_a_templated_uri_with_an_operation
    {
        private const string AmazonUriTemplate = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation={operation}&AssociateTag=myamzn-20";

        private const string ExpectedUri = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation=ItemSearch&AssociateTag=myamzn-20";

        private static dynamic _amazon;
        private static FakeHttpRequestFactory _requestFactory;

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();
            var templatedUriBuilder = new TemplatedUriBuilder
                {
                    UriTemplate = AmazonUriTemplate
                };
            var templatedUriRequestBuilder = new TemplatedUriRequestBuilder(_requestFactory) { Uri = AmazonUriTemplate };
            _amazon = new RestClient(templatedUriRequestBuilder, new ResponseProcessor(RestService.Xml));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => _amazon.ItemSearch();

        It should_merge_operationname_parameters_into_the_uri_template = () => ExpectedUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());
    }

    [Subject(typeof(RestClient))]
    public class When_using_a_templated_uri_with_an_operation_and_options
    {
        private const string AmazonUriTemplate = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation={operation}&AssociateTag=myamzn-20";

        private const string ExpectedUri = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation=ItemSearch&AssociateTag=myamzn-20&SearchIndex=Books&Keywords=Dynamic+Programming";

        private static dynamic _amazon;
        private static dynamic _searchOptions;
        private static FakeHttpRequestFactory _requestFactory;

       Establish context = () =>
       {
            _requestFactory = new FakeHttpRequestFactory();
           var templatedUriRequestBuilder = new TemplatedUriRequestBuilder(_requestFactory) { Uri = AmazonUriTemplate };
           _amazon = new RestClient(templatedUriRequestBuilder, new ResponseProcessor(RestService.Xml));

            _searchOptions = new JsonObject();
            _searchOptions.SearchIndex = "Books";
            _searchOptions.Keywords = "Dynamic Programming";                              
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => _amazon.ItemSearch(_searchOptions);

        It should_merge_operation_and_option_parameters_into_the_uri_template = () => ExpectedUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());
    }
 
    [Subject(typeof(RestClient))]
    public class When_using_a_templated_uri_with_ids_in_uri
    {
        private static FakeHttpRequestFactory _requestFactory;
        private static dynamic _bing;

        private const string BingSearchUri = "http://api.bing.net/json.aspx?AppId={appID}&Version=2.2&Market=en-US";
        private const string ExpectedUri = "http://api.bing.net/json.aspx?AppId=12345&Version=2.2&Market=en-US"; 
        private const string BingApiKey = "12345";

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();
            var templatedUriRequestBuilder = new TemplatedUriRequestBuilder(_requestFactory) { Uri = BingSearchUri };
            _bing = new RestClient(templatedUriRequestBuilder, new ResponseProcessor(RestService.Json));
            _bing.appID = BingApiKey;
        };

        Because we_make_a_call_to_an_api_via_rest_client = () => _bing.Invoke();

        It should_merge_properties_into_the_uri_template = () => ExpectedUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());
    }

    public class When_using_a_templated_uri_with_ids_and_missing_ids
    {
        private static FakeHttpRequestFactory _requestFactory;
        private static dynamic _bing;

        private const string BingSearchUri = "http://api.bing.net/json.aspx?AppId={appID}&Version=2.2&Market=en-US";
        private const string ExpectedUri = "http://api.bing.net/json.aspx?AppId=12345&Version=2.2&Market=en-US";
        private const string BingApiKey = "12345";
        private static Exception _exception;

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();
            var templatedUriRequestBuilder = new TemplatedUriRequestBuilder(_requestFactory) { Uri = BingSearchUri };
            _bing = new RestClient(templatedUriRequestBuilder, new ResponseProcessor(RestService.Json));
        };

        Because we_make_a_call_to_an_api_via_rest_client = () => _exception = Catch.Exception(() => _bing.Invoke());
        
        It should_throw_an_exception = () => _exception.ShouldBeOfType(typeof (ArgumentException));

        It should_contain_helpful_error_message = () => _exception.Message.ShouldEqual("You are missing one or more expected template parameters in the uri: http://api.bing.net/json.aspx?AppId={appID}&Version=2.2&Market=en-US");
    }

    [Subject(typeof(RestClient))]
    public class When_using_a_templated_uri_with_ids_in_uri_and_options
    {
        private static FakeHttpRequestFactory _requestFactory;
        private static dynamic _bing;
        private static dynamic _searchOptions;

        private const string BingSearchUri = "http://api.bing.net/json.aspx?AppId={appID}&Version=2.2&Market=en-US";
        private const string ExpectedUri = "http://api.bing.net/json.aspx?AppId=12345&Version=2.2&Market=en-US&Query=seattle&Sources=Web+Image&Web.Count=4&Image.Count=2"; 
        private const string BingApiKey = "12345";

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();
            var templatedUriRequestBuilder = new TemplatedUriRequestBuilder(_requestFactory) { Uri = BingSearchUri };
            _bing = new RestClient(templatedUriRequestBuilder, new ResponseProcessor(RestService.Json));
            _bing.appID = BingApiKey;

            _searchOptions = new JsonObject();
            _searchOptions.Query = "seattle";
            _searchOptions.Sources = new[] {"Web", "Image"};
            _searchOptions.Web = new JsonObject("Count", 4);
            _searchOptions.Image = new JsonObject("Count", 2);
        };

        Because we_make_a_call_to_an_api_via_rest_client = () => _bing.Invoke(_searchOptions);

        It should_merge_properties_into_the_uri_template = () => ExpectedUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());
    }

    [Subject(typeof(RestClient))]
    public class When_using_a_templated_uri_with_ids_in_uri_and_operation_and_options
    {
        private static dynamic _flickr;
        private static dynamic _searchOptions;
        private static FakeHttpRequestFactory _requestFactory;

        private const string FlickerSearchAPI = "http://api.flickr.com/services/rest/?method=flickr.{operation}&api_key={apiKey}&format=json&nojsoncallback=1";
        private const string ExpectedUri = "http://api.flickr.com/services/rest/?method=flickr.Photos.Search&api_key=123456&format=json&nojsoncallback=1";
        private const string FlickerAPIKey = "123456";

        private Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();
            var templatedUriRequestBuilder = new TemplatedUriRequestBuilder(_requestFactory) { Uri = FlickerSearchAPI };
            _flickr = new RestClient(templatedUriRequestBuilder, new ResponseProcessor(RestService.Json));
            _flickr.apiKey = FlickerAPIKey;

            dynamic _searchOptions = new JsonObject();
            _searchOptions.tags = "seattle";
            _searchOptions.per_page = 4;
        };

        Because we_make_a_call_to_an_api_via_the_rest_client = () => _flickr.Photos.Search(_searchOptions);

        It should_merge_properties_into_the_uri_template = () => ExpectedUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());
    }
}
