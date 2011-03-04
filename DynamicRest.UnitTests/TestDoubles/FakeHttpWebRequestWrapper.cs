using System;
using System.IO;
using System.Net;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest.UnitTests.TestDoubles
{
    public class FakeHttpWebRequestWrapper : IHttpRequest
    {
        private readonly Uri _uri;

        private HttpVerb _verb;

        private string _contentType;

        private string _requestBody;

        public FakeHttpWebRequestWrapper(Uri uri){
            _uri = uri;
        }

        public Uri RequestURI{
            get { return _uri; }
        }

        public string Accept{
            get { return ""; }
            set { }
        }

        public void AddCredentials(ICredentials credentials){
            
        }

        public void AddHeaders(WebHeaderCollection headers){
            
        }

        public void AddRequestBody(string contentType, string content) {
            this.Body = content;
            this.ContentType = contentType;
        }

        public string ContentType { get; set; }

        public string Body { get; set; }

        public void BeginGetResponse(Action<object> action, object asyncRequest){
           
        }

        public IHttpResponse EndGetResponse(object asyncRequest){
            return new FakeHttpWebResponseWrapper();
        }

        public IHttpResponse GetResponse(){
            return new FakeHttpWebResponseWrapper();
        }

        public Stream GetRequestStream(){
            return new MemoryStream();
        }

        public void SetContentHeaders(string contentType, int contentLength){
            
        }

        public HttpVerb HttpVerb
        {
            get {
                return _verb;
            }
            set {
                _verb = value;
            }
        }

        public string GetContentType() {
            return _contentType;
        }

        public string GetRequestBody() {
            return _requestBody;
        }
    }
}