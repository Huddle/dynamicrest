using System;
using System.IO;
using System.Net;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest
{
    public class ResponseProcessor : IProcessResponses
    {
        private readonly RestService _service;
        private readonly IBuildDynamicResults _builder;

        public ResponseProcessor(RestService service, IBuildDynamicResults resultBuilder) {
            _service = service;
            _builder = resultBuilder;
        }

        public void Process(IHttpResponse webResponse, RestOperation operation) {
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = webResponse.GetResponseStream();

                try
                {
                    object result = ProcessResponse(responseStream);
                    operation.Complete(result,
                        webResponse.StatusCode, webResponse.StatusDescription);
                }
                catch (Exception e)
                {
                    operation.Complete(new WebException(e.Message, e),
                        webResponse.StatusCode, webResponse.StatusDescription);
                }
            }
            else
            {
                operation.Complete(new WebException(webResponse.StatusDescription),
                    webResponse.StatusCode, webResponse.StatusDescription);
            }
        }

        private object ProcessResponse(Stream responseStream) {
            if (_service == RestService.Binary)
            {
                return responseStream;
            }

            dynamic result = null;
            try
            {
                var responseText = (new StreamReader(responseStream)).ReadToEnd();
                result = _builder.CreateResult(responseText, _service);
            }
            catch {}

            return result;
        }
    }
}