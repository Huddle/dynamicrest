using DynamicRest.UnitTests.TestDoubles;
using Machine.Specifications;

namespace DynamicRest.UnitTests.RestClients.Uris
{
    [Subject(typeof(RestClient))]
    public class When_using_a_templated_uri_with_an_operation
    {
        public const string AmazonUriTemplate = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation={operation}&AssociateTag=myamzn-20";

        public const string ExpectedUri = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation=ItemSearch&AssociateTag=myamzn-20";

        private static dynamic _amazon;
        private static dynamic _searchOptions;
        private static FakeHttpRequestFactory _requestFactory;

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();
            _amazon = new RestClient(_requestFactory, AmazonUriTemplate, RestService.Xml);
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => _amazon.ItemSearch(_searchOptions);

        private It should_merge_operationname_parameters_into_the_uri_template = () =>
        {
            ExpectedUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());
        };
    }

    [Subject(typeof(RestClient))]
    public class When_using_a_templated_uri_with_an_operation_and_options
    {
        public const string AmazonUriTemplate = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation={operation}&AssociateTag=myamzn-20";

        public const string ExpectedUri = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation=ItemSearch&AssociateTag=myamzn-20&SearchIndex=Books&Keywords=Dynamic+Programming";

        private static dynamic _amazon;
        private static dynamic _searchOptions;
        private static FakeHttpRequestFactory _requestFactory;

       Establish context = () =>
       {
            _requestFactory = new FakeHttpRequestFactory();
            _amazon = new RestClient(_requestFactory, AmazonUriTemplate, RestService.Xml);

            _searchOptions = new JsonObject();
            _searchOptions.SearchIndex = "Books";
            _searchOptions.Keywords = "Dynamic Programming";                              
        };

        Because we_make_get_call_to_an_api_via_rest_client = () => _amazon.ItemSearch(_searchOptions);

        It should_merge_operation_and_option_parameters_into_the_uri_template = () =>
        {
            ExpectedUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());
        };
    }
 
    [Subject(typeof(RestClient))]
    public class When_using_a_templated_uri_with_ids_in_uri
    {
        private static FakeHttpRequestFactory _requestFactory;
        private static dynamic _bing;

        public const string BingSearchUri = "http://api.bing.net/json.aspx?AppId={appID}&Version=2.2&Market=en-US";
        public const string ExpectedUri = "http://api.bing.net/json.aspx?AppId=12345&Version=2.2&Market=en-US"; 
        public const string BingApiKey = "12345";

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();
            _bing = new RestClient(_requestFactory, BingSearchUri, RestService.Json);
            _bing.appID = BingApiKey;
        };

        Because we_make_a_call_to_an_api_via_rest_client = () => _bing.Invoke();

        private It should_merge_properties_into_the_uri_template = () => ExpectedUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());

    }

    [Subject(typeof(RestClient))]
    public class When_using_a_templated_uri_with_ids_in_uri_and_options
    {
        private static FakeHttpRequestFactory _requestFactory;
        private static dynamic _bing;
        private static dynamic _searchOptions;

        public const string BingSearchUri = "http://api.bing.net/json.aspx?AppId={appID}&Version=2.2&Market=en-US";
        public const string ExpectedUri = "http://api.bing.net/json.aspx?AppId=12345&Version=2.2&Market=en-US&Query=seattle&Sources=Web+Image&Web.Count=4&Image.Count=2"; 
        public const string BingApiKey = "12345";

        Establish context = () =>
        {
            _requestFactory = new FakeHttpRequestFactory();
            _bing = new RestClient(_requestFactory, BingSearchUri, RestService.Json);
            _bing.appID = BingApiKey;

            _searchOptions = new JsonObject();
            _searchOptions.Query = "seattle";
            _searchOptions.Sources = new[]{"Web","Image"};
            _searchOptions.Web = new JsonObject("Count", 4);
            _searchOptions.Image = new JsonObject("Count", 2);
        };

        Because we_make_a_call_to_an_api_via_rest_client = () => _bing.Invoke(_searchOptions);

        private It should_merge_properties_into_the_uri_template = () => ExpectedUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());
    }
}
