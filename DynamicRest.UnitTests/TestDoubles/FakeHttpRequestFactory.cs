using System;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest.UnitTests.TestDoubles {

    internal class FakeHttpRequestFactory : IHttpRequestFactory {
        private IHttpResponse _httpResponse;

        internal FakeHttpWebRequestWrapper CreatedRequest {get; set; }

        public IHttpRequest Create(Uri uri) {
            CreatedRequest = new FakeHttpWebRequestWrapper(uri);
            if (_httpResponse != null)
            {
                CreatedRequest.Response = _httpResponse;
            }
            return CreatedRequest;
        }

        public void SetResponse(IHttpResponse httpResponse)
        {
             _httpResponse = httpResponse;
        }
    }
}