using System;
using System.IO;
using System.Net;
using System.Text;
using DynamicRest.Helpers;

namespace DynamicRest.HTTPInterfaces.HttpWebRequestWrappers {

    public class HttpWebRequestWrapper : IHttpRequest {

        private readonly HttpWebRequest _webRequest;

        public HttpWebRequestWrapper(HttpWebRequest webRequest) {
            _webRequest = webRequest;
        }

        public Uri RequestURI => _webRequest.RequestUri;

        public HttpVerb HttpVerb {
            get => _webRequest.Method.ToHttpVerb();
            set => _webRequest.Method = value.ToString();
        }

        public WebHeaderCollection Headers => _webRequest.Headers;

        public string Accept {
            get => _webRequest.Accept;
            set => _webRequest.Accept = value;
        }
        
        public string ContentType => _webRequest.ContentType;

        public bool AllowAutoRedirect {
            get => _webRequest.AllowAutoRedirect;
            set => _webRequest.AllowAutoRedirect = value;
        }

        public string UserAgent
        {
            get => _webRequest.UserAgent;
            set => _webRequest.UserAgent = value;
        }

        public int Timeout
        {
            get => _webRequest.Timeout;
            set => _webRequest.Timeout = value;
        }

        public IWebProxy Proxy
        {
            get => _webRequest.Proxy;
            set => _webRequest.Proxy = value;
        }

        public void AddCredentials(ICredentials credentials) {
            _webRequest.Credentials = credentials;
        }

        public void AddHeaders(WebHeaderCollection headers) {
            _webRequest.Headers.Add(headers);
        }

        public void AddRequestBody(string contentType, string content) {
            if (content != null) {
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                _webRequest.ContentType = contentType;
                _webRequest.ContentLength = bytes.Length;
                using (Stream requestStream = _webRequest.GetRequestStream()) {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public void BeginGetResponse(Action<object> action, object asyncRequest) {
            _webRequest.BeginGetResponse(ar => action(ar), asyncRequest);
        }

        public IHttpResponse EndGetResponse(object asyncRequest) {
            return new HttpWebResponseWrapper((HttpWebResponse) _webRequest.EndGetResponse((IAsyncResult) asyncRequest));
        }

        public IHttpResponse GetResponse() {
            return new HttpWebResponseWrapper(_webRequest.GetResponse() as HttpWebResponse);
        }
    }
}