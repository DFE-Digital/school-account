namespace Dfe.SchoolAccount.SignIn;

public sealed class DfESignInConfig : IDfESignInConfig
{
    public string APIServiceAudience { get; set; } = null!;

    public string APIServiceProxyUrl { get; set; } = null!;

    public string APIServiceSecret { get; set; } = null!;

    public string APIServiceUrl { get; set; } = null!;

    public string Authority { get; set; } = null!;

    public string CallbackUrl { get; set; } = null!;

    public string ClientId { get; set; } = null!;

    public string ClientSecret { get; set; } = null!;

    public int CookieExpiration { get; set; }

    public string CookieName { get; set; } = null!;

    public string Cryptography { get; set; } = null!;

    public bool GetClaimsFromUserInfoEndpoint { get; set; }

    public string MetaDataUrl { get; set; } = null!;

    public bool SaveTokens { get; set; }

    public IList<string> Scopes { get; set; } = null!;

    public string SignoutCallbackUrl { get; set; } = null!;

    public string SignoutRedirectUrl { get; set; } = null!;

    public bool SlidingExpiration { get; set; }

    public bool UseDfeSignin { get; set; }
}
