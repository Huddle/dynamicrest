using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using DynamicRest.HTTPInterfaces;

namespace DynamicRest.UnitTests.TestDoubles
{
    public class FakeHttpWebResponseWrapper : IHttpResponse, IDisposable {
        private WebHeaderCollection _webHeaderCollection = new WebHeaderCollection();
        private HttpStatusCode _httpStatusCode = HttpStatusCode.OK;
        private Stream _responseStream;
        private ResponseContent _responseContent = new ResponseContent();
        private long _contentLength = 0;

        public ResponseContent Content
        {
            set { _responseContent = value; }
        }

        public string ContentEncoding
        {
            get { return _responseContent.ContentEncoding; }
        }

        public long ContentLength
        {
            get { return _responseContent.ContentLength; }
        }

        public WebHeaderCollection Headers {
            get { return _webHeaderCollection; }
            set { _webHeaderCollection = value; }
        }

        public HttpStatusCode StatusCode {
            get { return _httpStatusCode; }
            set { _httpStatusCode = value; }
        }

        public string StatusDescription {
            get { return _httpStatusCode.ToString(); }
        }

        public Stream GetResponseStream()
        {
            _responseStream = _responseContent.GetResponseStream();
            return _responseStream;
        }

        public void Dispose()
        {
            if (_responseStream != null)
                _responseStream.Close();
        }


        public class ResponseContent{
            private string _rawContent = string.Empty;
            private readonly string _contentEncoding = "text/plain";
            private ResponseContentType _contentType;
            private object _graph;
            private Type _graphRootType;
            private long _contentLength = 0;

            public ResponseContent(){}

            public ResponseContent(string contentEncoding)
            {
                _contentEncoding = contentEncoding;
            }

            public long ContentLength
            {
                get { return _contentLength; }
            }

            public string ContentEncoding
            {
               get { return _contentEncoding; }
            }

            public void SetContent(string content){
                _contentType = ResponseContentType.Raw;
                _rawContent = content;
            }

            public void SetContent<T>(T responseContent)
            {
                _graph = responseContent;
                _graphRootType = typeof (T);
                _contentType = ResponseContentType.SerializedObject;
            }

            public Stream GetResponseStream()
            {
                if (_contentType == ResponseContentType.Raw){
                    return GetRawContentResponseStream();
                }

                return GetSerializedObjectResponseStream();
            }

            private Stream GetSerializedObjectResponseStream()
            {
                var serializer = new XmlSerializer(_graphRootType);
                Stream stream = new MemoryStream();
                serializer.Serialize(stream, _graph);

                _contentLength = stream.Length;

                stream.Position = 0;
                return stream;
            }

            private Stream GetRawContentResponseStream()
            {
                byte[] bytes = GetBytes();

                _contentLength = bytes.Length;

                var responseStream = new MemoryStream();

                responseStream.Write(bytes, 0, bytes.Length);

                responseStream.Seek(0, 0);

                return responseStream;
            }

            internal byte[] GetBytes()
           {
               return Encoding.UTF8.GetBytes(_rawContent);       
           }

            internal enum ResponseContentType{
                Raw,
                SerializedObject
            }
        }
    }
}