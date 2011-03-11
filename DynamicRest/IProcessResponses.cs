using DynamicRest.HTTPInterfaces;

namespace DynamicRest {

    public interface IProcessResponses {
        void Process(IHttpResponse webResponse, RestOperation operation);
    }
}