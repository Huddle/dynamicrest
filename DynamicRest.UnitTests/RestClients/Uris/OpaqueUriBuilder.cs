using System;

namespace DynamicRest.UnitTests.RestClients.Uris
{
    public class OpaqueUriBuilder : IBuildUris
    {
        public Uri CreateRequestUri(string operationName, JsonObject parameters)
        {
            return new Uri(UriTemplate);
        }

        public string UriTemplate { get; set; }

        public void SetUriTransformer(IRestUriTransformer uriTransformer)
        {
            throw new NotImplementedException();
        }
    }
}