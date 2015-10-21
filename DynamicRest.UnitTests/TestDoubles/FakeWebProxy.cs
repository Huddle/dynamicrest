using System;
using System.Net;

namespace DynamicRest.UnitTests.TestDoubles
{
    internal class FakeWebProxy : IWebProxy
    {
        readonly Uri _proxyUri;

        public FakeWebProxy(Uri proxyUri)
        {
            this._proxyUri = proxyUri;
        }

        public Uri GetProxy(Uri destination)
        {
            return this._proxyUri;
        }

        public bool IsBypassed(Uri host)
        {
            return true;
        }

        public ICredentials Credentials { get; set; }
    }
}