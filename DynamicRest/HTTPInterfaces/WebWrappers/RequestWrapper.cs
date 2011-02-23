using System;
using System.IO;
using System.Net;
using System.Text;

namespace DynamicRest.HTTPInterfaces.WebWrappers
{
    public class RequestWrapper : IHttpRequest
    {
        private readonly HttpWebRequest _webrequest;

        public RequestWrapper(HttpWebRequest webrequest)
        {
            _webrequest = webrequest;
        }

        public Uri RequestURI
        {
            get { return _webrequest.RequestUri; }
        }

        public void AddCredentials(ICredentials credentials)
        {
            _webrequest.Credentials = credentials;
        }

        public void AddHeaders(WebHeaderCollection headers)
        {
            throw new NotImplementedException();
        }
 
        public void AddRequestBody(string contentType, string content)
        {
            if (content != null)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                SetContentHeaders(contentType, bytes.Length);
                using (Stream requestStream = _webrequest.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
            }
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
            throw new NotImplementedException();
        }

        public void SetContentHeaders(string contentType, int contentLength)
        {
            throw new NotImplementedException();
        }

        public void SetHttpVerb(HttpVerb verb)
        {
            throw new NotImplementedException();
        }
    }
}