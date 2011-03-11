using System;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest.UnitTests.TestDoubles {

    internal class FakeHttpRequestFactory : IHttpRequestFactory {

        internal FakeHttpWebRequestWrapper CreatedRequest {get; set; }

        public IHttpRequest Create(Uri uri) {
            CreatedRequest = new FakeHttpWebRequestWrapper(uri);
            return CreatedRequest;
        }
    }
}