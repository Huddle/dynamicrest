using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest
{
    public class ResponseProcessor : IProcessResponses
    {
        private RestService _service;
        private WebHeaderCollection _responseHeaders;

        public ResponseProcessor(RestService service) {
            this._service = service;
        }

        public string GetResponseHeader(HttpResponseHeader header) {
            return _responseHeaders[header];
        }

        public void Process(IHttpResponse webResponse, RestOperation operation) {
            _responseHeaders = webResponse.Headers;
            if (webResponse.StatusCode == HttpStatusCode.OK)
            {
                Stream responseStream = webResponse.GetResponseStream();

                try
                {
                    object result = this.ProcessResponse(responseStream);
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
                string responseText = (new StreamReader(responseStream)).ReadToEnd();
                if (_service == RestService.Json)
                {
                    var jsonReader = new JsonReader(responseText);
                    result = jsonReader.ReadValue();
                }
                else
                {
                    //responseText = StripXmlnsRegex.Replace(responseText, String.Empty);
                    XDocument xmlDocument = XDocument.Parse(responseText);

                    result = new XmlNode(xmlDocument.Root);
                }
            }
            catch {}

            return result;
        }
    }

    public interface IProcessResponses {
        void Process(IHttpResponse webResponse, RestOperation operation);
    }
}