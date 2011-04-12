using DynamicRest.HTTPInterfaces;
using System.Net;

namespace DynamicRest {
    public interface IProcessResponses {
        void Process(IHttpResponse webResponse, RestOperation operation);
        void Process(HttpWebResponse webResponse, RestOperation operation);
    }
}