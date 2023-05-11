namespace Dfe.SchoolAccount.SignIn.Tests.Mocks;

using System.Net;

public static class HttpClientMocks
{
    public static HttpClient CreateHttpClientMock(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        return new HttpClient(new HttpMessageHandlerMock(handler));
    }

    public static HttpClient CreateHttpClientMock(HttpResponseMessage response)
    {
        return CreateHttpClientMock(message => response);
    }

    public static HttpClient CreateHttpClientMock()
    {
        return CreateHttpClientMock(new HttpResponseMessage(HttpStatusCode.OK));
    }

    private sealed class HttpMessageHandlerMock : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, HttpResponseMessage> handler;

        public HttpMessageHandlerMock(Func<HttpRequestMessage, HttpResponseMessage> handler)
        {
            this.handler = handler;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(this.handler(request));
        }
    }
}
