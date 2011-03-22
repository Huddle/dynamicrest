using DynamicRest.HTTPInterfaces;

namespace DynamicRest {
    using System.Net;

    public interface IProcessResponses {
        void Process(IHttpResponse webResponse, RestOperation operation);
        void Process(HttpWebResponse webResponse, RestOperation operation);
    }
}