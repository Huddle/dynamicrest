using Machine.Specifications;

namespace DynamicRest.UnitTests.Json
{
    public class When_a_response_contains_a_collection
    {
        static StandardResultBuilder _resultBuilder;
        static dynamic _response;

        Establish context = () =>
        {
            _resultBuilder = new StandardResultBuilder();
        };

        Because the_response_is_created = () => { _response = _resultBuilder.CreateResult(_xml, RestService.Json); };

        It should_contain_the_media_0_url = () => (_response.item.media[0].url as string).ShouldEqual("http://media0url");
        It should_contain_the_media_1_url = () => (_response.item.media[1].url as string).ShouldEqual("http://media1url");
        It should_contain_the_image_0_url = () => (_response.item.images.image[0].src as string).ShouldEqual("http://image0url");
        It should_contain_the_image_1_url = () => (_response.item.images.image[1].src as string).ShouldEqual("http://image1url");
        It should_contain_the_link_using_array_access = () => (_response.item.link[0] as string).ShouldEqual("http://www.bbc.co.uk/go/rss/int/news/-/news/world-africa-12673956");

        static string _xml = @"
           {
                item:{
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
                            {
                                src:'http://image0url'
                            },
                            {
                                src:'http://image1url'
                            }
                        ]
                    }
                }
        }";
    }
}