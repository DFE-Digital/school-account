namespace Dfe.SchoolAccount.SignIn;

public interface IDfESignInConfig
{
    /// <summary>
    /// Typically "signin.education.gov.uk". Please Refer GitHub Documentation https://github.com/DFE-Digital/login.dfe.public-api#get-user-access-to-service
    /// </summary>
    string APIServiceAudience { get; }

    /// <summary>
    /// Absolute URL Path. Set ONLY if Required By Middleware
    /// </summary>
    string APIServiceProxyUrl { get; }

    string APIServiceSecret { get; }

    /// <summary>
    /// Absolute URL Path. Environment Specifc 
    /// </summary>
    string APIServiceUrl { get; }

    string Authority { get; }

    string CallbackUrl { get; }

    string ClientId { get; }

    string ClientSecret { get; }

    int CookieExpiration { get; }

    string CookieName { get; }

    /// <summary>
    /// Currently only supports HMAC types
    /// </summary>
    string Cryptography { get; }

    bool GetClaimsFromUserInfoEndpoint { get; }

    string MetaDataUrl { get; }

    bool SaveTokens { get; }

    IList<string> Scopes { get; }

    string SignoutCallbackUrl { get; }

    string SignoutRedirectUrl { get; }

    bool SlidingExpiration { get; }

    bool UseDfeSignin { get; }
}
