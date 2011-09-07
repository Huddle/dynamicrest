using System;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using DynamicRest.Json;
using DynamicRest.Xml;

namespace DynamicRest {

    public class StandardResultBuilder : IBuildDynamicResults {
        readonly RestService _serviceType;

        public StandardResultBuilder(RestService serviceType) {
            _serviceType = serviceType;
        }

        public object CreateResult(string responseText) {
            return _serviceType == RestService.Json 
                       ? GetResultFromJson(responseText) 
                       : GetResultFromXml(responseText);
        }

        public static object GetResultFromXml(string responseText) {
            var xmlDocument = XDocument.Parse(responseText);
            dynamic result = new XmlNode(xmlDocument.Root);
            return result;
        }

        public static object GetResultFromJson(string responseText) {
            var jsonReader = new JsonReader(responseText);
            dynamic result = jsonReader.ReadValue();
            return result;
        }

        public object ProcessResponse(Stream responseStream) {
            if (_serviceType == RestService.Binary) {
                return responseStream;
            }

            dynamic result = null;
            try {
                var responseText = (new StreamReader(responseStream)).ReadToEnd();
                result = CreateResult(responseText);
            }
            catch {}

            return result;
        }
    }
}