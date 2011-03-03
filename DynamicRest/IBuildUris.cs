using System;

namespace DynamicRest
{
    public interface IBuildUris
    {
        Uri CreateRequestUri(string operationName, JsonObject parameters);
        string UriTemplate { get; set; }
        
        void SetUriTransformer(IRestUriTransformer uriTransformer);
    }
}