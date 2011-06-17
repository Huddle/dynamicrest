using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest 
{
    public class ResponseProcessor : IProcessResponses 
    {
        private readonly IBuildDynamicResults _builder;

        public ResponseProcessor(IBuildDynamicResults resultBuilder) 
        {
            _builder = resultBuilder;
        }

        public void Process(IHttpResponse webResponse, RestOperation operation) 
        {
            try 
            {
                using (var responseStream = webResponse.GetResponseStream())
                {
                    if (IsGzippedReponse(webResponse.Headers["Content-Encoding"]))
                    {
                        using (var gzipResponseStream = new GZipStream(responseStream, CompressionMode.Decompress))
                        {
                            ProcessResponseStream(webResponse, gzipResponseStream, operation);
                        }
                    }
                    else
                    {
                        ProcessResponseStream(webResponse, responseStream, operation);
                    }
                }
            }
            catch(Exception e) 
            {
                operation.Complete(null, new WebException(e.Message, e),
                    webResponse.StatusCode, webResponse.StatusDescription, webResponse.Headers, string.Empty);
            }
        }

        private static bool IsGzippedReponse(string contentEncodingHeader)
        {
            if (!string.IsNullOrEmpty(contentEncodingHeader))
            {
                if (contentEncodingHeader.ToLower().Contains("gzip"))
                {
                    return true;
                }
            }

            return false;
        }

        private void ProcessResponseStream(IHttpResponse webResponse, Stream responseStream, RestOperation operation)
        {
            var responseText = (new StreamReader(responseStream)).ReadToEnd();

            if (webResponse.StatusCode == HttpStatusCode.OK || webResponse.StatusCode == HttpStatusCode.Created) 
            {
                object result = _builder.ProcessResponse(responseStream);
                operation.Complete(result, null,
                                   webResponse.StatusCode, webResponse.StatusDescription, webResponse.Headers, responseText);
            }
            else 
            {
                object result = _builder.ProcessResponse(responseStream);
                operation.Complete(result, new WebException(webResponse.StatusDescription),
                                   webResponse.StatusCode, webResponse.StatusDescription, webResponse.Headers, responseText);
            }
        }
    }
}