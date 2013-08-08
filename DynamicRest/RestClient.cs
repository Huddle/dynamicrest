// RestClient.cs
// DynamicRest provides REST service access using C# 4.0 dynamic programming.
// The latest information and code for the project can be found at
// https://github.com/NikhilK/dynamicrest
//
// This project is licensed under the BSD license. See the License.txt file for
// more information.
//

using System;
using System.Dynamic;
using System.Net;
using DynamicRest.HTTPInterfaces;
using DynamicRest.HTTPInterfaces.WebWrappers;
using DynamicRest.Json;

namespace DynamicRest {

    public sealed class RestClient : DynamicObject {
    
        private readonly string _operationGroup;
        private readonly WebHeaderCollection _responseHeaders = new WebHeaderCollection();
        private readonly IBuildRequests _requestBuilder;
        private readonly IProcessResponses _responseProcessor;

        public RestClient(IBuildRequests requestBuilder, IProcessResponses responseProcessor) {
            _responseProcessor = responseProcessor;
            _requestBuilder = requestBuilder;
        }

        private RestClient(RestClient restClient, string operationGroup)
            : this(restClient._requestBuilder, restClient._responseProcessor) {
            _operationGroup = operationGroup;
        }

        private RestOperation PerformOperation(string operationName, IProcessResponses responseProcessor, params object[] args) {
            JsonObject argsObject = null;
            if ((args != null) && (args.Length != 0)) {
                argsObject = (JsonObject)args[0];
            }

            var operation = new RestOperation();

            IHttpRequest webRequest = _requestBuilder.CreateRequest(operationName, argsObject);
            
            InterpretResponse(responseProcessor, operation, () => webRequest.GetResponse());
            
            return operation;
        }

        private RestOperation PerformOperationAsync(string operationName, IProcessResponses responseProcessor, params object[] args) {
            JsonObject argsObject = null;
            if ((args != null) && (args.Length != 0)) {
                argsObject = (JsonObject)args[0];
            }

            var operation = new RestOperation();

            IHttpRequest webRequest = _requestBuilder.CreateRequest(operationName, argsObject);

            webRequest.BeginGetResponse(ar => InterpretResponse(responseProcessor, operation, () => webRequest.EndGetResponse(ar)), null);

            return operation;
        }

        private void InterpretResponse(IProcessResponses responseProcessor, RestOperation operation, Func<IHttpResponse> returnsResponse)
        {
            try {
                var webResponse = returnsResponse();
                responseProcessor.Process(webResponse, operation);
                _responseHeaders.Add(webResponse.Headers);
            }
            catch (WebException webException)
            {
                if (webException.Status != WebExceptionStatus.ProtocolError)
                {
                    throw;
                }
                var response = (HttpWebResponse) webException.Response;
                var responseWrapper = new HttpWebResponseWrapper(response);
                responseProcessor.Process(responseWrapper, operation);
                _responseHeaders.Add(response.Headers);
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            object value = _requestBuilder.ParametersStore.GetParameter(binder.Name);
            if (value != null) {
                result = value;
                return true;
            }

            string operationGroup = binder.Name;
            if (_operationGroup != null) {
                operationGroup = _operationGroup + "." + operationGroup;
            }

            var operationGroupClient = new RestClient(this, operationGroup);

            result = operationGroupClient;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result) {
            bool async = false;

            string operation = binder.Name;
            if (operation.EndsWith("Async", StringComparison.Ordinal)) {
                async = true;
                operation = operation.Substring(0, operation.Length - 5);
            }

            if (_operationGroup != null) {
                operation = _operationGroup + "." + operation;
            }

            if (async == false) {
                result = PerformOperation(operation, _responseProcessor, args);
            }
            else {
                result = PerformOperationAsync(operation, _responseProcessor, args);
            }
            return true;
        }
        
        public override bool TrySetMember(SetMemberBinder binder, object value) {
            _requestBuilder.ParametersStore.SetParameter(binder.Name, value);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result) {
            if(indexes.Length == 1 && indexes[0].GetType() == typeof(HttpResponseHeader)) {
                result = _responseHeaders[(HttpResponseHeader)indexes[0]];
                return true;
            }
            return base.TryGetIndex(binder, indexes, out result);
        }
    }
}
