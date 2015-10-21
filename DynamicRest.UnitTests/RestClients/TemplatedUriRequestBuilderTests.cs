using System;
using DynamicRest.Json;
using DynamicRest.UnitTests.TestDoubles;
using Machine.Specifications;

namespace DynamicRest.UnitTests.RestClients {

    [Subject(typeof(TemplatedUriRequestBuilder))]
    public class When_using_a_templated_uri_with_an_operation {
        private const string AmazonUriTemplate = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation={operation}&AssociateTag=myamzn-20";
        private const string ExpectedUri = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation=ItemSearch&AssociateTag=myamzn-20";
        private static dynamic client;
        private static FakeHttpRequestFactory requestFactory;

        Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();
            var templatedUriRequestBuilder = new TemplatedUriRequestBuilder(requestFactory) { Uri = AmazonUriTemplate };
            client = new RestClient(templatedUriRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => client.ItemSearch();

        It should_merge_operationname_parameters_into_the_uri_template = () => ExpectedUri.ShouldEqual(requestFactory.CreatedRequest.RequestURI.ToString());
    }

    [Subject(typeof(TemplatedUriRequestBuilder))]
    public class When_using_a_templated_uri_with_an_operation_and_options {
        private const string AmazonUriTemplate = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation={operation}&AssociateTag=myamzn-20";
        private const string ExpectedUri = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation=ItemSearch&AssociateTag=myamzn-20&SearchIndex=Books&Keywords=Dynamic+Programming";
        private static dynamic client;
        private static dynamic searchOptions;
        private static FakeHttpRequestFactory requestFactory;

       Establish context = () =>
       {
            requestFactory = new FakeHttpRequestFactory();
           var templatedUriRequestBuilder = new TemplatedUriRequestBuilder(requestFactory) { Uri = AmazonUriTemplate };
           client = new RestClient(templatedUriRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Xml)));

            searchOptions = new JsonObject();
            searchOptions.SearchIndex = "Books";
            searchOptions.Keywords = "Dynamic Programming";                              
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => client.ItemSearch(searchOptions);

        It should_merge_operation_and_option_parameters_into_the_uri_template = () => ExpectedUri.ShouldEqual(requestFactory.CreatedRequest.RequestURI.ToString());
    }

    [Subject(typeof(TemplatedUriRequestBuilder))]
    public class When_using_a_templated_uri_with_ids_in_uri {
        private const string BingSearchUri = "http://api.bing.net/json.aspx?AppId={appID}&Version=2.2&Market=en-US";
        private const string ExpectedUri = "http://api.bing.net/json.aspx?AppId=12345&Version=2.2&Market=en-US"; 
        private const string BingApiKey = "12345";
        private static FakeHttpRequestFactory requestFactory;
        private static dynamic client;

        Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();
            var templatedUriRequestBuilder = new TemplatedUriRequestBuilder(requestFactory) { Uri = BingSearchUri };
            client = new RestClient(templatedUriRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Json)));
            client.appID = BingApiKey;
        };

        Because we_make_a_call_to_an_api_via_rest_client = () => client.Invoke();

        It should_merge_properties_into_the_uri_template = () => ExpectedUri.ShouldEqual(requestFactory.CreatedRequest.RequestURI.ToString());
    }

    [Subject(typeof(TemplatedUriRequestBuilder))]
    public class When_using_a_templated_uri_with_ids_and_missing_ids {
        private static FakeHttpRequestFactory requestFactory;
        private static dynamic bing;
        private const string BingSearchUri = "http://api.bing.net/json.aspx?AppId={appID}&Version=2.2&Market=en-US";
        private const string ExpectedUri = "http://api.bing.net/json.aspx?AppId=12345&Version=2.2&Market=en-US";
        private const string BingApiKey = "12345";
        private static Exception exception;

        Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();
            var templatedUriRequestBuilder = new TemplatedUriRequestBuilder(requestFactory) { Uri = BingSearchUri };
            bing = new RestClient(templatedUriRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Json)));
        };

        Because we_make_a_call_to_an_api_via_rest_client = () => exception = Catch.Exception(() => bing.Invoke());
        
        It should_throw_an_exception = () => exception.ShouldBeAssignableTo(typeof (ArgumentException));

        It should_contain_helpful_error_message = () => exception.Message.ShouldEqual("You are missing one or more expected template parameters in the uri: http://api.bing.net/json.aspx?AppId={appID}&Version=2.2&Market=en-US");
    }

    [Subject(typeof(TemplatedUriRequestBuilder))]
    public class When_using_a_templated_uri_with_ids_in_uri_and_options {
        private static FakeHttpRequestFactory requestFactory;
        private static dynamic bing;
        private static dynamic searchOptions;
        private const string BingSearchUri = "http://api.bing.net/json.aspx?AppId={appID}&Version=2.2&Market=en-US";
        private const string ExpectedUri = "http://api.bing.net/json.aspx?AppId=12345&Version=2.2&Market=en-US&Query=seattle&Sources=Web+Image&Web.Count=4&Image.Count=2"; 
        private const string BingApiKey = "12345";

        Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();
            var templatedUriRequestBuilder = new TemplatedUriRequestBuilder(requestFactory) { Uri = BingSearchUri };
            bing = new RestClient(templatedUriRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Json)));
            bing.appID = BingApiKey;

            searchOptions = new JsonObject();
            searchOptions.Query = "seattle";
            searchOptions.Sources = new[] {"Web", "Image"};
            searchOptions.Web = new JsonObject("Count", 4);
            searchOptions.Image = new JsonObject("Count", 2);
        };

        Because we_make_a_call_to_an_api_via_rest_client = () => bing.Invoke(searchOptions);

        It should_merge_properties_into_the_uri_template = () => ExpectedUri.ShouldEqual(requestFactory.CreatedRequest.RequestURI.ToString());
    }

    [Subject(typeof(TemplatedUriRequestBuilder))]
    public class When_using_a_templated_uri_with_ids_in_uri_and_operation_and_options {
        private static dynamic flickr;
        private static dynamic searchOptions;
        private static FakeHttpRequestFactory requestFactory;
        private const string FlickrSearchApi = "http://api.flickr.com/services/rest/?method=flickr.{operation}&api_key={apiKey}&format=json&nojsoncallback=1";
        private const string ExpectedUri = "http://api.flickr.com/services/rest/?method=flickr.Photos.Search&api_key=123456&format=json&nojsoncallback=1";
        private const string FlickrApiKey = "123456";

        private Establish context = () =>
        {
            requestFactory = new FakeHttpRequestFactory();
            var templatedUriRequestBuilder = new TemplatedUriRequestBuilder(requestFactory) { Uri = FlickrSearchApi };
            flickr = new RestClient(templatedUriRequestBuilder, new ResponseProcessor(new StandardResultBuilder(RestService.Json)));
            flickr.apiKey = FlickrApiKey;
            
            dynamic searchOptions = new JsonObject();
            searchOptions.tags = "seattle";
            searchOptions.per_page = 4;
        };

        Because we_make_a_call_to_an_api_via_the_rest_client = () => flickr.Photos.Search(searchOptions);

        It should_merge_properties_into_the_uri_template = () => ExpectedUri.ShouldEqual(requestFactory.CreatedRequest.RequestURI.ToString());
    }
}
