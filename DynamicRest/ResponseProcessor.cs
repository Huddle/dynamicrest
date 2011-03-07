using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using DynamicRest.HTTPInterfaces;
using DynamicRest.Json;
using DynamicRest.Xml;

namespace DynamicRest
{
    public class ResponseProcessor : IProcessResponses
    {
        private RestService _service;

        public ResponseProcessor(RestService service) {
            this._service = service;
        }

        public void Process(IHttpResponse webResponse, RestOperation operation) {
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
}