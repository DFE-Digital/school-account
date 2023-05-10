namespace Dfe.SchoolAccount.SignIn.PublicApi;

using Dfe.SchoolAccount.SignIn.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

/// <summary>
/// An implementation of <see cref="IDfePublicApi"/> that initiates requests via a
/// <see cref="HttpClient"/>.
/// </summary>
public sealed class DfePublicApi : IDfePublicApi
{
    private readonly HttpClient httpClient;
    private readonly IDfePublicApiConfiguration configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="DfePublicApi"/> class.
    /// </summary>
    /// <param name="configuration">Configuration for the DfE Sign-in public API.</param>
    /// <param name="client">Client for making HTTP requests.</param>
    /// <exception cref="ArgumentException">
    /// If <paramref name="configuration"/> or <paramref name="client"/> is <c>null</c>.
    /// </exception>
    public DfePublicApi(IDfePublicApiConfiguration configuration, HttpClient client)
    {
        if (string.IsNullOrEmpty(configuration.BaseUrl)) {
            throw new ArgumentException("Invalid API service URL", nameof(configuration));
        }
        if (string.IsNullOrEmpty(configuration.BaseUrl)) {
            throw new ArgumentException("Invalid API service URL", nameof(configuration));
        }

        this.configuration = configuration;

        this.httpClient = client;
        this.httpClient.BaseAddress = new Uri(configuration.BaseUrl);
        this.httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.CreateBearerToken()}");
    }

    /// <inheritdoc/>
    public async Task<UserAccessToService> GetUserAccessToService(string userId, string organisationId)
    {
        if (userId == null) {
            throw new ArgumentNullException(nameof(userId));
        }
        if (organisationId == null) {
            throw new ArgumentNullException(nameof(organisationId));
        }

        var endpoint = $"services/{this.configuration.ClientId}/organisations/{organisationId}/users/{userId}";
        var response = await this.httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode) {
            throw new DfePublicApiException(response.StatusCode);
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var userAccessToService = JsonSerializer.Deserialize<UserAccessToService>(responseContent)!;
        return userAccessToService;
    }

    private string CreateBearerToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(this.configuration.ApiSecret);
        var tokenDescriptor = new SecurityTokenDescriptor {
            Audience = this.configuration.ServiceAudience,
            Issuer = this.configuration.ClientId,
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
