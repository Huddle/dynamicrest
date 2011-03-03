// RestClient.cs
// DynamicRest provides REST service access using C# 4.0 dynamic programming.
// The latest information and code for the project can be found at
// https://github.com/NikhilK/dynamicrest
//
// This project is licensed under the BSD license. See the License.txt file for
// more information.
//

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest {

    public sealed class RestClient : DynamicObject {
        private static readonly Regex StripXmlnsRegex =
            new Regex(@"(xmlns:?[^=]*=[""][^""]*[""])",
                      RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled | RegexOptions.CultureInvariant);

        private readonly RestService _service;
        private readonly string _operationGroup;
        private WebHeaderCollection _responseHeaders = new WebHeaderCollection();
        private readonly IBuildRequests _requestBuilder;
        private readonly TemplatedUriBuilder _templatedUriBuilder = new TemplatedUriBuilder();

        private Uri _requestUri;

        public RestClient(IBuildRequests requestBuilder, RestService service)
        {
            _service = service;
            _requestBuilder = requestBuilder;

        }

        public RestClient(IBuildRequests requestBuilder, TemplatedUriBuilder templatedUriBuilder, RestService service)
            : this(requestBuilder, service)
        {
            this._templatedUriBuilder = templatedUriBuilder;
        }

        private RestClient(IBuildRequests requestBuilder, TemplatedUriBuilder templatedUriBuilder, RestService service, string operationGroup)
            : this(requestBuilder, templatedUriBuilder, service)
        {
            _operationGroup = operationGroup;
        }


        //public RestClient ForUri(Uri requestUri){
        //    _requestUri = requestUri;
        //    return this;
        //}

        private RestOperation PerformOperation(string operationName, params object[] args) {
            JsonObject argsObject = null;
            if ((args != null) && (args.Length != 0)) {
                argsObject = (JsonObject)args[0];
            }

            var operation = new RestOperation();

            var uri = BuildUri(operationName, argsObject);
            IHttpRequest webRequest = _requestBuilder.CreateRequest(uri, operationName, argsObject);

            try {
                IHttpResponse webResponse = webRequest.GetResponse();
                _responseHeaders = webResponse.Headers;
                if (webResponse.StatusCode == HttpStatusCode.OK || webResponse.StatusCode == HttpStatusCode.Created) {
                    Stream responseStream = webResponse.GetResponseStream();

                    try {
                        object result = ProcessResponse(responseStream);
                        operation.Complete(result,
                                           webResponse.StatusCode, webResponse.StatusDescription);
                    }
                    catch (Exception e) {
                        operation.Complete(new WebException(e.Message, e),
                                           webResponse.StatusCode, webResponse.StatusDescription);
                    }
                }
                else {
                    operation.Complete(new WebException(webResponse.StatusDescription),
                                       webResponse.StatusCode, webResponse.StatusDescription);
                }
            }
            catch (WebException webException) {
                var response = (HttpWebResponse)webException.Response;
                operation.Complete(webException, response.StatusCode, response.StatusDescription);
            }

            return operation;
        }

        private RestOperation PerformOperationAsync(string operationName, params object[] args) {
            JsonObject argsObject = null;
            if ((args != null) && (args.Length != 0)) {
                argsObject = (JsonObject)args[0];
            }

            var operation = new RestOperation();

            var uri = BuildUri(operationName, argsObject);
            IHttpRequest webRequest = _requestBuilder.CreateRequest(uri, operationName, argsObject);

            webRequest.BeginGetResponse(ar => {
                try {
                    var webResponse = (HttpWebResponse)webRequest.EndGetResponse(ar);
                    _responseHeaders = webResponse.Headers;
                    if (webResponse.StatusCode == HttpStatusCode.OK) {
                        Stream responseStream = webResponse.GetResponseStream();

                        try {
                            object result = ProcessResponse(responseStream);
                            operation.Complete(result,
                                               webResponse.StatusCode, webResponse.StatusDescription);
                        }
                        catch (Exception e) {
                            operation.Complete(new WebException(e.Message, e),
                                               webResponse.StatusCode, webResponse.StatusDescription);
                        }
                    }
                    else {
                        operation.Complete(new WebException(webResponse.StatusDescription),
                                           webResponse.StatusCode, webResponse.StatusDescription);
                    }
                }
                catch (WebException webException) {
                    var response = (HttpWebResponse)webException.Response;
                    operation.Complete(webException, response.StatusCode, response.StatusDescription);
                }
            }, null);

            return operation;
        }

        private object ProcessResponse(Stream responseStream) {
            if (_service == RestService.Binary) {
                return responseStream;
            }

            dynamic result = null;
            try {
                string responseText = (new StreamReader(responseStream)).ReadToEnd();
                if (_service == RestService.Json) {
                    var jsonReader = new JsonReader(responseText);
                    result = jsonReader.ReadValue();
                }
                else {
                    responseText = StripXmlnsRegex.Replace(responseText, String.Empty);
                    XDocument xmlDocument = XDocument.Parse(responseText);

                    result = new XmlNode(xmlDocument.Root);
                }
            }
            catch{
            }

            return result;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) 
        {
            object value = this._templatedUriBuilder.GetParameter(binder.Name);
            if (value != null)
            {
                result = value;
                return true;
            }

            string operationGroup = binder.Name;
            if (_operationGroup != null) {
                operationGroup = _operationGroup + "." + operationGroup;
            }

            var operationGroupClient = new RestClient(_requestBuilder, this._templatedUriBuilder, _service, operationGroup);

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
                result = PerformOperation(operation, args);
            }
            else {
                result = PerformOperationAsync(operation, args);
            }
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value){
            this._templatedUriBuilder.SetParameter(binder.Name, value);
            return true;
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            if(indexes.Length == 1 && indexes[0].GetType() == typeof(HttpResponseHeader))
            {
                result = _responseHeaders[(HttpResponseHeader)indexes[0]];
                return true;
            }
            return base.TryGetIndex(binder, indexes, out result);
        }

        public RestClient WithCredentials(ICredentials credentials) {
            if (credentials == null) {
                throw new ArgumentNullException("credentials");
            }

            _requestBuilder.Credentials = credentials;
            return this;
        }

        public RestClient WithHeader(HttpRequestHeader headerType, string value){
            _requestBuilder.AddHeader(headerType, value);
            return this;
        }

        public RestClient WithAuthorization(OAuth oAuth){
            WithHeader(HttpRequestHeader.Authorization, oAuth.Token);
            return this;
        }

        public RestClient WithXmlBody(string xml){
            _requestBuilder.Body = xml;
            return this;
        }

        public RestClient WithContentType(string contentType)
        {
            _requestBuilder.ContentType = contentType;
             return this;
        }

        public RestClient WithUriTransformer(IRestUriTransformer uriTransformer) {
            if (uriTransformer == null) {
                throw new ArgumentNullException("uriTransformer");
            }

            this._templatedUriBuilder.SetUriTransformer(uriTransformer);
            return this;
        }

        public IEnumerable<HttpResponseHeader> ResponseHeaders
        {
            get
            {
                foreach (HttpResponseHeader header in _responseHeaders)
                    yield return header;
                yield break;
            }
        }

        public string this[HttpResponseHeader index]
        {
            get { return _responseHeaders[index]; }
        }

        private Uri BuildUri(string operationName, JsonObject parameters)
        {
            if (_templatedUriBuilder == null)
            {
                throw new InvalidOperationException("You ust set a template builder before trying to build the Uri");
            }

            if (_requestUri == null)
            {
                _requestUri = _templatedUriBuilder.CreateRequestUri(operationName, parameters);
            }

            return _requestUri;
        }
    }
}
