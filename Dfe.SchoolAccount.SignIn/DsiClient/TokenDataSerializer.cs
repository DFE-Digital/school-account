namespace Dfe.SchoolAccount.SignIn.DsiClient;

using System.Text.Json;

public sealed class TokenDataSerializer : ITokenDataSerializer
{
    public string Serialize(object obj)
    {
        return JsonSerializer.Serialize(obj);
    }
}
