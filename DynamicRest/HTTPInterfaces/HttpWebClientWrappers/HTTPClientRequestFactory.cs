using System;
using System.Net.Http;

namespace DynamicRest.HTTPInterfaces.HttpWebClientWrappers
{
    class HTTPClientRequestFactory : IHttpRequestFactory
    {
        public IHttpRequest Create(Uri uri)
        {
            return new HTTPClientRequestWrapper(new HttpClient(uri));
        }
    }
}
