using System;
using DynamicRest.Json;
using Machine.Specifications;

namespace DynamicRest.UnitTests.Json {

    [Subject(typeof(StandardResultBuilder))]
    public class When_a_response_contains_a_collection {

        static StandardResultBuilder _resultBuilder;
        static dynamic _response;

        Establish context = () =>
        {
            _resultBuilder = new StandardResultBuilder(RestService.Json);
        };

        Because the_response_is_created = () => { _response = _resultBuilder.CreateResult(_json); };

        It should_contain_the_media_0_url = () => (_response.item.media[0].url as string).ShouldEqual("http://media0url");
        It should_contain_the_media_1_url = () => (_response.item.media[1].url as string).ShouldEqual("http://media1url");
        It should_contain_the_image_0_url = () => (_response.item.images.image[0].src as string).ShouldEqual("http://image0url");
        It should_contain_the_image_1_url = () => (_response.item.images.image[1].src as string).ShouldEqual("http://image1url");
        It should_contain_the_link_using_array_access = () => (_response.item.link[0] as string).ShouldEqual("http://www.bbc.co.uk/go/rss/int/news/-/news/world-africa-12673956");

        static string _json = @"{
                                  item: {
                                    title:'Gaddafi renews attack on rebels',
                                    description:'Forces loyal to Libyan leader Col Muammar Gaddafi launch fresh air strikes on the rebel-held Ras Lanuf, as they try to retake the oil-rich town.',
                                    link:[
                                      'http://www.bbc.co.uk/go/rss/int/news/-/news/world-africa-12673956'
                                    ],
                                    guid:'http://www.bbc.co.uk/news/world-africa-12673956',
                                    pubdate:'Tue, 08 Mar 2011 11:21:16 GMT',
                                    media:[
                                      { url:'http://media0url' , src: 'dfsdfdfsfsd'},
                                      { url:'http://media1url' , src: 'dfsdfdfsfsd'}
                                    ],
                                    images:{
                                      image:[
                                        { src:'http://image0url' },
                                        { src:'http://image1url' }
                                      ]
                                    }
                                  }
                                }";
    }

    [Subject(typeof(JsonObject))]
    public class When_accessing_a_non_existing_property_on_a_json_property {

        static StandardResultBuilder _resultBuilder;
        static dynamic _response;

        Establish context = () =>
        {
            _resultBuilder = new StandardResultBuilder(RestService.Json);
            _response = _resultBuilder.CreateResult(_json);
        };

        Because the_response_is_created = () => { _thrownException = Catch.Exception(() => { var junk = _response.item.desc; }); };

        It should_give_an_exception_with_a_sensible_error_message_from_json_object = () =>
            _thrownException.Message.ShouldEqual("No member named 'desc' found in the response.");

        static string _json = @"{
                                  item: {
                                    title:'Gaddafi renews attack on rebels',
                                    description:'Forces loyal to Libyan leader Col Muammar Gaddafi launch fresh air strikes on the rebel-held Ras Lanuf, as they try to retake the oil-rich town.',
                                    link:['http://www.bbc.co.uk/go/rss/int/news/-/news/world-africa-12673956'],
                                    guid:'http://www.bbc.co.uk/news/world-africa-12673956',
                                    pubdate:'Tue, 08 Mar 2011 11:21:16 GMT',
                                    media:[
                                      { url:'http://media0url' , src: 'dfsdfdfsfsd'},
                                      { url:'http://media1url' , src: 'dfsdfdfsfsd'}
                                    ],
                                    images:{
                                      image:[
                                        { src:'http://image0url' },
                                        { src:'http://image1url' }
                                      ]
                                    }
                                  }
                                }";

        private static Exception _thrownException;
    }


    [Subject(typeof(JsonObject))]
    public class When_accessing_a_non_existing_property_on_a_json_array {

        static StandardResultBuilder _resultBuilder;
        static Exception _thrownException;
        static dynamic _response;
        static string _json;

        Establish context = () =>
        {
            _json = @"{ 
                        item:{
                          images:[
                            { src:'http://image0url' },
                            { src:'http://image1url' }
                          ]
                        }
                      }";

            _resultBuilder = new StandardResultBuilder(RestService.Json);
            _response = _resultBuilder.CreateResult(_json);
        };

        Because the_response_is_created = () => { _thrownException = Catch.Exception(() => { var junk = _response.item.images.doesntexist; }); };

        It should_give_an_exception_with_a_sensible_error_message_from_json_object = () =>
            _thrownException.Message.ShouldEqual("No member named 'doesntexist' found in the response.");
    }

    [Subject(typeof(StandardResultBuilder))]
    public class When_a_response_is_empty
    {

        static StandardResultBuilder _resultBuilder;
        static dynamic _response;

        Establish context = () =>
        {
            _resultBuilder = new StandardResultBuilder(RestService.Json);
        };

        Because the_response_is_created = () => { _response = _resultBuilder.CreateResult(_json); };

        It should_return_empty_json_object = () => (_response as JsonObject).ShouldEqual(new JsonObject());

        static string _json = @"";
    }
}