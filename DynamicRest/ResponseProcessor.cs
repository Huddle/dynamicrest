using System;
using System.IO;
using System.Net;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest {

    public class ResponseProcessor : IProcessResponses {

        private readonly IBuildDynamicResults _builder;

        public ResponseProcessor(IBuildDynamicResults resultBuilder) {
            _builder = resultBuilder;
        }

        public void Process(IHttpResponse webResponse, RestOperation operation) {
            if (webResponse.StatusCode == HttpStatusCode.OK) {
                Stream responseStream = webResponse.GetResponseStream();

                try {
                    object result = _builder.ProcessResponse(responseStream);
                    operation.Complete(result,
                        webResponse.StatusCode, webResponse.StatusDescription);
                }
                catch (Exception e) {
                    operation.Complete(new WebException(e.Message, e),
                        webResponse.StatusCode, webResponse.StatusDescription);
                }
            }
            else {
                operation.Complete(new WebException(webResponse.StatusDescription),
                    webResponse.StatusCode, webResponse.StatusDescription);
            }
        }
    }
}