namespace Dfe.SchoolAccount.SignIn;

public interface IDfeSignInConfiguration
{
    /// <summary>
    /// Absolute URL Path. Set ONLY if Required By Middleware
    /// </summary>
    string APIServiceProxyUrl { get; }

    string Authority { get; }

    string CallbackUrl { get; }

    string ClientId { get; }

    string ClientSecret { get; }

    int CookieExpiration { get; }

    string CookieName { get; }

    bool GetClaimsFromUserInfoEndpoint { get; }

    string MetaDataUrl { get; }

    bool SaveTokens { get; }

    IList<string> Scopes { get; }

    string SignoutCallbackUrl { get; }

    string SignoutRedirectUrl { get; }

    bool SlidingExpiration { get; }

    bool DiscoverRolesWithPublicApi { get; }
}
