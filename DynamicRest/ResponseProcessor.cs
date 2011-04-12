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
            try {
                Stream responseStream = webResponse.GetResponseStream();

                if (webResponse.StatusCode == HttpStatusCode.OK || webResponse.StatusCode == HttpStatusCode.Created) {
                        object result = _builder.ProcessResponse(responseStream);
                        operation.Complete(result, null,
                            webResponse.StatusCode, webResponse.StatusDescription, webResponse.Headers);
                }
                else {
                        object result = _builder.ProcessResponse(responseStream);
                        operation.Complete(result, new WebException(webResponse.StatusDescription),
                            webResponse.StatusCode, webResponse.StatusDescription, webResponse.Headers);
                }
            }
            catch(Exception e) {
                operation.Complete(null, new WebException(e.Message, e),
                    webResponse.StatusCode, webResponse.StatusDescription, webResponse.Headers);
            }
        }

        public void Process(HttpWebResponse webResponse, RestOperation operation) {
            try {
                Stream responseStream = webResponse.GetResponseStream();

                if (webResponse.StatusCode == HttpStatusCode.OK || webResponse.StatusCode == HttpStatusCode.Created) {
                    object result = _builder.ProcessResponse(responseStream);
                    operation.Complete(result, null,
                        webResponse.StatusCode, webResponse.StatusDescription, webResponse.Headers);
                }
                else {
                    object result = _builder.ProcessResponse(responseStream);
                    operation.Complete(result, new WebException(webResponse.StatusDescription),
                        webResponse.StatusCode, webResponse.StatusDescription, webResponse.Headers);
                }
            }
            catch (Exception e) {
                operation.Complete(null, new WebException(e.Message, e),
                    webResponse.StatusCode, webResponse.StatusDescription, webResponse.Headers);
            }
        }
    }
}