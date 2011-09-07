using System;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest.Testing {
    public class FakeHttpRequestFactory : IHttpRequestFactory {
        private IHttpResponse _httpResponse;

        public FakeHttpWebRequestWrapper CreatedRequest {get; set; }

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