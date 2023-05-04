namespace Dfe.SchoolAccount.SignIn.DsiClient;

public sealed class TokenData : ITokenData
{
    public IDictionary<string, object> Header { get; set; }

    public IDictionary<string, object> Payload { get; set; }

    public TokenData() : this(null, null) { }

    public TokenData(IDictionary<string, object>? header, IDictionary<string, object>? payload)
    {
        this.Header = header ?? new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        this.Payload = payload ?? new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    }
}
