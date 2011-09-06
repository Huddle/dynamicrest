using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace DynamicRest.HTTPInterfaces.HttpWebClientWrappers
{
    class HTTPClientRequestWrapper : IHttpRequest
    {
        private readonly HttpClient _client;
        private HttpMethod _method;
        private HttpContentHeaders _contentHeaders;
        private byte[] _bytes;

        public HTTPClientRequestWrapper(HttpClient client)
        {
            _client = client;
            Headers = new WebHeaderCollection();
        }

        public Uri RequestURI { get { return _client.BaseAddress; } }
        public HttpVerb HttpVerb
        {
            get
            {
                return (HttpVerb) Enum.Parse(typeof (HttpVerb), _method.ToString());
            }
            set
            {
                _method = new HttpMethod(value.ToString().ToUpper());
            }
        }

        public WebHeaderCollection Headers { get; set; }
        public string Accept { get; set; }
        public string ContentType { get; set; }
        public bool AllowAutoRedirect { get; set; }

        public void AddCredentials(ICredentials credentials)
        {
            throw new NotImplementedException();
        }

        public void AddHeaders(WebHeaderCollection headers)
        {
            Headers = headers;
        }

        public void AddRequestBody(string contentType, string content)
        {
            _contentHeaders.ContentType = new MediaTypeHeaderValue(contentType);
            _bytes = Encoding.UTF8.GetBytes(content);
        }

        public void BeginGetResponse(Action<object> action, object asyncRequest)
        {
            throw new NotImplementedException();
        }

        public IHttpResponse EndGetResponse(object asyncRequest)
        {
            throw new NotImplementedException();
        }

        public IHttpResponse GetResponse()
        {
            var request = new HttpRequestMessage(_method, string.Empty);
            request.Headers.
            request.Content = new ByteArrayContent(_bytes);


            return WrapResponse(_client.Send(request));
        }
    }
}
