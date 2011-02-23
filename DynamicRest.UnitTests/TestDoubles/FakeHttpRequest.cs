using System;
using System.IO;
using System.Net;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest.UnitTests.TestDoubles
{
    public class FakeHttpRequest : IHttpRequest
    {
        private readonly Uri _uri;

        public FakeHttpRequest(Uri uri)
        {
            _uri = uri;
        }

        public Uri RequestURI
        {
            get { return _uri; }
        }

        public void AddCredentials(ICredentials credentials)
        {
            
        }

        public void AddHeaders(WebHeaderCollection headers)
        {
            
        }

        public void AddRequestBody(string contentType, string content)
        {
           
        }

        public void BeginGetResponse(Action<object> action, object asyncRequest)
        {
           
        }

        public IHttpResponse EndGetResponse(object asyncRequest)
        {
            return new FakeHttpResponse();
        }

        public IHttpResponse GetResponse()
        {
            return new FakeHttpResponse();
        }

        public Stream GetRequestStream()
        {
            return new MemoryStream();
        }

        public void SetContentHeaders(string contentType, int contentLength)
        {
            
        }

        public void SetHttpVerb(HttpVerb verb)
        {
            
        }
    }
}