using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace DynamicRest.HTTPInterfaces.WebWrappers
{
    public class RequestFactory : IHttpRequestFactory
    {
        public IHttpRequest Create(Uri uri)
        {
            return new RequestWrapper((HttpWebRequest) HttpWebRequest.Create(uri));
        }
    }
}
