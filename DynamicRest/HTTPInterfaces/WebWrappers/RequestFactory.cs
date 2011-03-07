using System;
using System.Net;

namespace DynamicRest.HTTPInterfaces.WebWrappers
{
    public class RequestFactory : IHttpRequestFactory
    {
        public IHttpRequest Create(Uri uri)
        {
            return new HttpWebRequestWrapper((HttpWebRequest) HttpWebRequest.Create(uri));
        }
    }
}
