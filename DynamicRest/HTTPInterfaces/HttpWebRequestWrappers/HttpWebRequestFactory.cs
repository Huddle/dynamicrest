using System;
using System.Net;

namespace DynamicRest.HTTPInterfaces.HttpWebRequestWrappers {

    public class HttpWebRequestFactory : IHttpRequestFactory
    {
        public IHttpRequest Create(Uri uri) {
            return new HttpWebRequestWrapper((HttpWebRequest) WebRequest.Create(uri));
        }
    }
}
