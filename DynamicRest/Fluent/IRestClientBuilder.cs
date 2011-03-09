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
    }
}