namespace Dfe.SchoolAccount.SignIn.PublicApi;

using System.Net;

public sealed class DfePublicApiException : Exception
{
    public DfePublicApiException(HttpStatusCode statusCode)
        : base($"API responded with status code {statusCode}")
    {
    }
}
