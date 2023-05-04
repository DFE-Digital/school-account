namespace Dfe.SchoolAccount.SignIn.DsiClient;

/// <summary>
/// Currently supports variants of HMAC Algorithm 
/// </summary>
public sealed class JsonWebAlgorithm : IJsonWebAlgorithm
{
    public readonly Dictionary<string, string> Algorithm;

    public JsonWebAlgorithm()
    {
        this.Algorithm = new Dictionary<string, string> {
            { "HMACSHA1", "HS1" },
            { "HMACSHA256", "HS256" },
            { "HMACSHA384", "HS384" },
            { "HMACSHA512", "HS512" }
        };
    }

    public string GetAlgorithm(string algorithm)
    {
        if (!this.Algorithm.ContainsKey(algorithm)) {
            throw new KeyNotFoundException("Cannot find equivalent JSON Web Algorithm");
        }

        return this.Algorithm[algorithm];
    }
}
