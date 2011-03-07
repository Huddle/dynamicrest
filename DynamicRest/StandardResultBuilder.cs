using System.Xml.Linq;
using DynamicRest.Json;
using DynamicRest.Xml;

namespace DynamicRest
{
    public class StandardResultBuilder : IBuildDynamicResults
    {
        public object CreateResult(string responseText, RestService serviceType)
        {
            return serviceType == RestService.Json 
                       ? GetResultFromJson(responseText) 
                       : GetResultFromXml(responseText);
        }

        public static object GetResultFromXml(string responseText)
        {
            var xmlDocument = XDocument.Parse(responseText);
            dynamic result = new XmlNode(xmlDocument.Root);
            return result;
        }

        public static object GetResultFromJson(string responseText)
        {
            var jsonReader = new JsonReader(responseText);
            dynamic result = jsonReader.ReadValue();
            return result;
        }
    }
}