using System;
using System.Collections.Generic;
using System.Net;

namespace DynamicRest.Fluent {

    public interface IRestClientBuilder {

        dynamic Build();

        IRestClientBuilder WithContentType(string contentType);
        IRestClientBuilder WithRequestBuilder(IBuildRequests requestBuilder);
        IRestClientBuilder WithUri(string uri);
        IRestClientBuilder WithBody(string body);
        IRestClientBuilder WithAcceptHeader(string acceptType);
        IRestClientBuilder WithOAuth2Token(string token);
        IRestClientBuilder WithResponseProcessor(IProcessResponses responseProcessor);
        IRestClientBuilder WithNoAcceptHeader();
        IRestClientBuilder WithAutoRedirect(bool autoRedirect);
        IRestClientBuilder WithAcceptEncodingHeader(string acceptEncodingType);
        IRestClientBuilder WithIfModifiedSinceDate(DateTime ifModifiedSince);
        IRestClientBuilder WithHeaders(Dictionary<HttpRequestHeader, string> headers);
        IRestClientBuilder WithCustomHeaders(Dictionary<string, string> headers);
        IRestClientBuilder WithUserAgent(string userAgent);
    }
}