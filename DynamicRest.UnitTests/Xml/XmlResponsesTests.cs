using Machine.Specifications;

namespace DynamicRest.UnitTests.Xml
{
    public class When_a_response_contains_a_collection
    {
        static StandardResultBuilder _resultBuilder;
        static dynamic _response;

        Establish context = () =>
        {
            _resultBuilder = new StandardResultBuilder();
        };

        Because the_response_is_created = () => { _response = _resultBuilder.CreateResult(_xml, RestService.Xml); };

        It should_contain_the_media_0_url = () => (_response.item.media[0].url as string).ShouldEqual("http://media0url");
        It should_contain_the_media_1_url = () => (_response.item.media[1].url as string).ShouldEqual("http://media1url");
        It should_contain_the_image_0_url = () => (_response.item.images.image[0].src as string).ShouldEqual("http://image0url");
        It should_contain_the_image_1_url = () => (_response.item.images.image[1].src as string).ShouldEqual("http://image1url");
        It should_contain_the_link_using_array_access = () => (_response.item.link[0].Value as string).ShouldEqual("http://www.bbc.co.uk/go/rss/int/news/-/news/world-africa-12673956");
        It should_contain_the_attachment_title = () => (_response.item.attachments.attachment[0].title.Value as string).ShouldEqual("this is the title");
        It should_contain_the_numbers_of_attachments = () => ((int)_response.item.attachments.Count).ShouldEqual(1);

        It should_work_when_a_refence_is_used_as_an_array = () =>
        {
            var linkAsArray = _response.item.link;
            ((string)linkAsArray[0].Value).ShouldEqual("http://www.bbc.co.uk/go/rss/int/news/-/news/world-africa-12673956");
        };

        static string _xml = @"
            <news>
              <item> 
                <title>Gaddafi renews attack on rebels</title>  
                <description>Forces loyal to Libyan leader Col Muammar Gaddafi launch fresh air strikes on the rebel-held Ras Lanuf, as they try to retake the oil-rich town.</description>
                <link>http://www.bbc.co.uk/go/rss/int/news/-/news/world-africa-12673956</link>  
                <link>http://www.bbc.co.uk/go/rss/int/news/-/news/world-africa-12673956</link>  
                <guid>http://www.bbc.co.uk/news/world-africa-12673956</guid>  
                <pubDate>Tue, 08 Mar 2011 11:21:16 GMT</pubDate>  
                <media url=""http://media0url""/>  
                <media url=""http://media1url""/> 
                <images>
                  <image src=""http://image0url"" />
                  <image src=""http://image1url"" />
                </images>
                <attachments>
                    <attachment>
                        <title>this is the title</title>
                    </attachment>
                </attachments>
              </item>
            </news>";
    }
}