using System;
using System.IO;
using System.Net;
using System.Text;

using DynamicRest.Helpers;

namespace DynamicRest.HTTPInterfaces.WebWrappers {

    public class HttpWebRequestWrapper : IHttpRequest {

        private readonly HttpWebRequest _webrequest;

        public HttpWebRequestWrapper(HttpWebRequest webrequest) {
            _webrequest = webrequest;
        }

        public Uri RequestURI {
            get { return _webrequest.RequestUri; }
        }

        public HttpVerb HttpVerb {
            get {
                return _webrequest.Method.ToHttpVerb();
            }
            set {
                _webrequest.Method = value.ToString();
            }
        }

        public WebHeaderCollection Headers {
            get { return _webrequest.Headers; }
        }
 
        public string Accept {
            get {
                return _webrequest.Accept;
            }
            set {
                _webrequest.Accept = value;
            }
        }
        
        public string ContentType {
            get {
                return _webrequest.ContentType;
            }
        }

        public bool AllowAutoRedirect {
            get { return _webrequest.AllowAutoRedirect; }
            set { _webrequest.AllowAutoRedirect = value; }
        }

        public void AddCredentials(ICredentials credentials) {
            _webrequest.Credentials = credentials;
        }

        public void AddHeaders(WebHeaderCollection headers) {
            _webrequest.Headers.Add(headers);
        }

        public void AddRequestBody(string contentType, string content) {
            if (content != null) {
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                _webrequest.ContentType = contentType;
                _webrequest.ContentLength = bytes.Length;
                using (Stream requestStream = _webrequest.GetRequestStream()) {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
            }
        }

        public void BeginGetResponse(Action<object> action, object asyncRequest) {
            _webrequest.BeginGetResponse(ar => action(ar), asyncRequest);
        }

        public IHttpResponse EndGetResponse(object asyncRequest) {
            return new HttpWebResponseWrapper((HttpWebResponse) _webrequest.EndGetResponse((IAsyncResult) asyncRequest));
        }

        public IHttpResponse GetResponse() {
            return new HttpWebResponseWrapper(_webrequest.GetResponse() as HttpWebResponse);
        }
    }
}