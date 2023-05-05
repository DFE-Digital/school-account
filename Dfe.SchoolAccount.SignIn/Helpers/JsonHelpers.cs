namespace Dfe.SchoolAccount.SignIn.Helpers;

using System.Text.Json;

public static class JsonHelpers
{
    public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static string Serialize<T>(T value)
    {
        return JsonSerializer.Serialize<T>(value, JsonSerializerOptions);
    }

    public static T? Deserialize<T>(string json)
    {
        return JsonSerializer.Deserialize<T>(json, JsonSerializerOptions);
    }
}
