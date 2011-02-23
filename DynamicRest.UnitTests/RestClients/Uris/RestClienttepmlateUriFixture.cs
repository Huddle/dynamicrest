using DynamicRest.UnitTests.TestDoubles;
using Machine.Specifications;

namespace DynamicRest.UnitTests.RestClients.Uris
{
    [Subject(typeof(RestClient))]
    public class When_get_from_a_templated_uri
    {
        public const string AmazonUriTemplate = "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation={operation}&AssociateTag=myamzn-20";

        public const string ExpectedUri =
            "http://ecs.amazonaws.com/onca/xml?Service=AWSECommerceService&Version=2009-03-31&Operation=ItemSearch&AssociateTag=myamzn-20&SearchIndex=Books&Keywords=Dynamic+Programming";
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

        private It should_merge_template_parameters_into_the_uri = () =>
        {
            ExpectedUri.ShouldEqual(_requestFactory.CreatedRequest.RequestURI.ToString());
        };
    }
}
