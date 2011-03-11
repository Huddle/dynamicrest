using System;

namespace DynamicRest.HTTPInterfaces {

    public interface IHttpRequestFactory {
        IHttpRequest Create(Uri uri);
    }
}