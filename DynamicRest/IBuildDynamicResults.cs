using System.IO;

namespace DynamicRest {

    public interface IBuildDynamicResults {
        object CreateResult(string responseText);
        BuilderResponse ProcessResponse(Stream responseStream);
        RestService ServiceType { get; }
    }
}