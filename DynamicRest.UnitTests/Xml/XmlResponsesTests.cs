using System;
using DynamicRest.Xml;
using Machine.Specifications;

namespace DynamicRest.UnitTests.Xml
{
    [Subject(typeof(StandardResultBuilder))]
    public class When_a_response_contains_a_collection {

        static StandardResultBuilder _resultBuilder;
        static dynamic _response;

        Establish context = () =>
        {
            _resultBuilder = new StandardResultBuilder(RestService.Xml);
        };

        Because the_response_is_created = () => { _response = _resultBuilder.CreateResult(_xml); };

        It should_contain_the_media_0_url = () => (_response.item.media[0].url as string).ShouldEqual("http://media0url");
        It should_contain_the_media_1_url = () => (_response.item.media[1].url as string).ShouldEqual("http://media1url");
        It should_contain_the_image_0_url = () => (_response.item.images.image[0].src as string).ShouldEqual("http://image0url");
        It should_contain_the_image_1_url = () => (_response.item.images.image[1].src as string).ShouldEqual("http://image1url");
        It should_contain_the_link_using_array_access = () => (_response.item.link[0].Value as string).ShouldEqual("http://www.bbc.co.uk/go/rss/int/news/-/news/world-africa-12673956");
        It should_contain_the_attachment_title = () => (_response.item.attachments.attachment[0].title.Value as string).ShouldEqual("this is the title");
        It should_contain_the_numbers_of_attachments = () => ((int)_response.item.attachments.Count).ShouldEqual(1);

        It should_contain_the_pubdate =
            () => ((DateTime)_response.item.pubDate).ShouldEqual(new DateTime(2011, 3, 8, 11, 21, 16));

        It should_work_when_a_refence_is_used_as_an_array = () =>
        {
            var linkAsArray = _response.item.link;
            ((string)linkAsArray[0].Value).ShouldEqual("http://www.bbc.co.uk/go/rss/int/news/-/news/world-africa-12673956");
        };

        static string _xml = @"
            <news>
              <item> 
                <title>Gaddafi renews attack on rebels</title>
                <version>5</version>  
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
    
    [Subject(typeof(XmlNode))]
    public class When_accessing_a_non_existing_element {

        static StandardResultBuilder _resultBuilder;
        static dynamic _response;

        Establish context = () =>
        {
            _resultBuilder = new StandardResultBuilder(RestService.Xml);
            _response = _resultBuilder.CreateResult(_xml);
        };

        private Because the_response_is_created = () => _thrownException = Catch.Exception(() => { var junk = _response.item.desc; });
        
        It should_work_when_a_refence_is_used_as_an_array = () => 
            _thrownException.Message.ShouldEqual("No element or attribute named 'desc' found in the response.");

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

        private static Exception _thrownException;
    }


    namespace When_casting_strings_to_datetimes
    {
        [Subject(typeof(XmlString))]
        public class When_a_string_is_a_valid_date
        {
            Establish context = () => The_string = new XmlString("2011-03-08T11:21:16Z");

            Because we_parse_as_a_date = () => The_result = (DateTime)The_string;

            It should_have_the_expected_value = () => The_result.ShouldEqual(new DateTime(2011, 3, 8, 11, 21, 16));

            private static XmlString The_string;
            private static DateTime The_result;
        }

        [Subject(typeof(XmlString))]
        public class When_parsing_fails
        {
            Establish context = () => The_string = new XmlString("I am not a valid string");

            Because we_parse_as_a_date = () => The_result = Catch.Exception(() => Console.Write((DateTime)The_string));

            private It should_have_the_expected_value = () => The_result.ShouldBeOfType<FormatException>();

            private static XmlString The_string;
            private static Exception The_result;
        }
    }

    namespace When_casting_strings_to_integers
    {
        [Subject(typeof(XmlString))]
        public class When_a_string_is_a_valid_int
        {
            Establish context = () => The_string = new XmlString("8787645");

            Because we_parse_as_an_int = () => The_result = (int)The_string;

            It should_have_the_expected_value = () => The_result.ShouldEqual(8787645);

            private static XmlString The_string;
            private static int The_result;
        }

        [Subject(typeof(XmlString))]
        public class When_a_string_is_an_overflow
        {
            Establish context = () => The_string = new XmlString( (((long)int.MaxValue) + 1).ToString());

            Because we_parse_as_an_int = () => The_result = Catch.Exception(() => Console.Write((int)The_string));

            private It should_have_the_expected_value = () => The_result.ShouldBeOfType<OverflowException>();

            private static XmlString The_string;
            private static Exception The_result;
        }


        [Subject(typeof(XmlString))]
        public class When_parsing_fails
        {
            Establish context = () => The_string = new XmlString("I am not a valid string");

            Because we_parse_as_an_int= () => The_result = Catch.Exception(() => Console.Write((int)The_string));

            private It should_have_the_expected_value = () => The_result.ShouldBeOfType<FormatException>();

            private static XmlString The_string;
            private static Exception The_result;
        }
    }

    namespace When_casting_strings_to_longs
    {
        [Subject(typeof(XmlString))]
        public class When_a_string_is_a_valid_long
        {
            Establish context = () => The_string = new XmlString("8787645");

            Because we_parse_as_a_long = () => The_result = (long)The_string;

            It should_have_the_expected_value = () => The_result.ShouldEqual(8787645);

            private static XmlString The_string;
            private static long The_result;
        }

        [Subject(typeof(XmlString))]
        public class When_a_string_is_an_overflow
        {
            Establish context = () => The_string = new XmlString((long.MaxValue)+"1");

            Because we_parse_as_a_long = () => The_result = Catch.Exception(() => Console.Write((long)The_string));

            private It should_have_the_expected_value = () => The_result.ShouldBeOfType<OverflowException>();

            private static XmlString The_string;
            private static Exception The_result;
        }


        [Subject(typeof(XmlString))]
        public class When_parsing_fails
        {
            Establish context = () => The_string = new XmlString("I am not a valid string");

            Because we_parse_as_a_long = () => The_result = Catch.Exception(() => Console.Write((long)The_string));

            private It should_have_the_expected_value = () => The_result.ShouldBeOfType<FormatException>();

            private static XmlString The_string;
            private static Exception The_result;
        }
    }
 
    namespace When_casting_strings_to_bools
    {
        public class When_parsing_the_empty_string
        {
            Because we_parse_string_empty =
                () => Result = Catch.Exception(() => Console.Write((bool) new XmlString(string.Empty)));

            It should_throw_format_exception = () => Result.ShouldBeOfType<FormatException>();

            protected static Exception Result { get; set; }
        }

        public class When_parsing_the_zero_string
        {
            Because we_parse_string_empty =
                () => Result = (bool) new XmlString("0");

            It should_be_false = () => Result.ShouldBeFalse();

            protected static bool Result { get; set; }
        }

        public class When_parsing_negative_one
        {
            Because we_parse_string_empty =
                () => Result = (bool)new XmlString("-1");

            It should_be_false = () => Result.ShouldBeFalse();

            protected static bool Result { get; set; }
        }

        public class When_parsing_one
        {
            Because we_parse_string_empty =
                () => Result = (bool)new XmlString("1");

            It should_be_false = () => Result.ShouldBeTrue();

            protected static bool Result { get; set; }
        }

        public class When_parsing_true
        {
            Because we_parse_string_empty =
                () => Result = (bool)new XmlString("true");

            It should_be_false = () => Result.ShouldBeTrue();

            protected static bool Result { get; set; }
        }

        public class When_parsing_false
        {
            Because we_parse_string_empty =
                () => Result = (bool)new XmlString("false");

            It should_be_false = () => Result.ShouldBeFalse();

            protected static bool Result { get; set; }
        }

        public class When_parsing_weird_casing
        {
            Because we_parse_string_empty =
                () => Result = (bool)new XmlString("tRUe");

            It should_be_false = () => Result.ShouldBeTrue();

            protected static bool Result { get; set; }
        }
    }
}