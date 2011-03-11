using System.IO;

namespace DynamicRest {

    public interface IBuildDynamicResults {
        object CreateResult(string responseText);
        object ProcessResponse(Stream responseStream);
    }
}