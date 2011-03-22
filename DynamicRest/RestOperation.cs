// RestOperation.cs
// DynamicRest provides REST service access using C# 4.0 dynamic programming.
// The latest information and code for the project can be found at
// https://github.com/NikhilK/dynamicrest
//
// This project is licensed under the BSD license. See the License.txt file for
// more information.
//

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace DynamicRest {

    public sealed class RestOperation {

        private object _result;
        private Exception _error;
        private HttpStatusCode _statusCode;
        private string _statusMessage;
        private bool _completed;
        private SynchronizationContext _syncContext;
        private List<RestCallback> _callbacks;

        private WebHeaderCollection _responseHeaders;

        internal RestOperation() {
            _syncContext = SynchronizationContext.Current;
        }

        public Exception Error {
            get {
                return _error;
            }
        }

        public bool IsCompleted {
            get {
                return _completed;
            }
        }

        public dynamic Result {
            get {
                return _result;
            }
        }

        public HttpStatusCode StatusCode {
            get {
                return _statusCode;
            }
        }

        public string StatusMessage {
            get {
                return _statusMessage;
            }
        }

        public void Callback(RestCallback callback) {
            if (callback == null) {
                throw new ArgumentNullException("callback");
            }

            if (_completed) {
                callback();
            }

            if (_callbacks == null) {
                _callbacks = new List<RestCallback>();
            }
            _callbacks.Add(callback);
        }

        public string GetHeader(HttpResponseHeader headerType) {
            return _responseHeaders[headerType];
        }

        internal void Complete(object result, HttpStatusCode statusCode, string statusMessage, WebHeaderCollection headers) {
            Complete(result, null, statusCode, statusMessage, headers);
        }

        internal void Complete(Exception error, HttpStatusCode statusCode, string statusMessage, WebHeaderCollection headers) {
            this.Complete(null, error, statusCode, statusMessage, headers);
        }

        private void Complete(object result, Exception error, HttpStatusCode statusCode, string statusMessage, WebHeaderCollection headers) {
            _result = result;
            _error = error;
            _statusCode = statusCode;
            _statusMessage = statusMessage;
            _completed = true;
            _responseHeaders = headers;

            if (_callbacks != null) {
                RestCallback[] callbacksCopy = _callbacks.ToArray();
                _callbacks.Clear();

                if (_syncContext == null) {
                    InvokeCallbacks(callbacksCopy);
                }
                else {
                    _syncContext.Post(InvokeCallbacks, callbacksCopy);
                }
            }
        }

        private static void InvokeCallbacks(object state) {
            RestCallback[] callbacks = (RestCallback[])state;

            foreach (RestCallback callback in callbacks) {
                callback();
            }
        }
    }
}
