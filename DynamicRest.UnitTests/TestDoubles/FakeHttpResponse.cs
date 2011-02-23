using System;
using System.IO;
using System.Net;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest.UnitTests.TestDoubles
{
    public class FakeHttpResponse : IHttpResponse
    {
        public WebHeaderCollection Headers
        {
            get { return new WebHeaderCollection(); }
        }

        public HttpStatusCode StatusCode
        {
            get { return HttpStatusCode.OK; }
        }

        public string StatusDescription
        {
            get { return string.Empty; }
        }

        public Stream GetResponseStream()
        {
            return new MemoryStream();
        }
    }
}