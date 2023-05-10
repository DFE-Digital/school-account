namespace Dfe.SchoolAccount.SignIn.Helpers;

using System.Text.Json;

/// <summary>
/// Internal helper methods for serializing and deserializing JSON encoded data
/// from DfE Sign-in API's.
/// </summary>
/// <remarks>
/// <para>This is achieved by providing consistent JSON serialization options.</para>
/// </remarks>
internal static class JsonHelpers
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
