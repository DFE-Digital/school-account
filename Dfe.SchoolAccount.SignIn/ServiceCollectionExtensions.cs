namespace Dfe.SchoolAccount.SignIn;

using System.Net;
using System.Security.Claims;
using Dfe.SchoolAccount.SignIn.Constants;
using Dfe.SchoolAccount.SignIn.Helpers;
using Dfe.SchoolAccount.SignIn.Models;
using Dfe.SchoolAccount.SignIn.PublicApi;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add support for the DfE sign-in public API.
    /// </summary>
    /// <remarks>
    /// <para>This is used to gather additional information such as user roles
    /// (see <see cref="IDfeSignInConfiguration.DiscoverRolesWithPublicApi"/>).</para>
    /// <para>This should be called before calling <see cref="AddDfeSignInAuthentication"/>
    /// when both services are being used.</para>
    /// </remarks>
    /// <param name="configuration">Configuration options.</param>
    /// <seealso cref="AddDfeSignInAuthentication"/>
    public static void AddDfeSignInPublicApi(this IServiceCollection services, IDfePublicApiConfiguration configuration)
    {
        services.AddSingleton<IDfePublicApiConfiguration>(configuration);
        services.AddSingleton<IDfePublicApi, DfePublicApi>();
    }

    /// <summary>
    /// Add support for DfE sign-in authentication using Open ID.
    /// </summary>
    /// <param name="configuration">Configuration options.</param>
    /// <seealso cref="AddDfeSignInPublicApi"/>
    public static void AddDfeSignInAuthentication(this IServiceCollection services, IDfeSignInConfiguration configuration)
    {
        services.AddSingleton<IDfeSignInConfiguration>(configuration);

        services.AddHttpClient();
        services.AddHttpClient<DfePublicApi>()
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler {
                AllowAutoRedirect = true,
                UseDefaultCredentials = false,
                PreAuthenticate = true,
                Proxy = !string.IsNullOrWhiteSpace(configuration.APIServiceProxyUrl)
                    ? new WebProxy(new Uri(configuration.APIServiceProxyUrl, UriKind.Absolute))
                    : null
            });
        services.AddHttpContextAccessor();

        services.AddAuthentication(sharedOptions => {
            sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
            .AddOpenIdConnect(options => {
                options.ClientId = configuration.ClientId;
                options.ClientSecret = configuration.ClientSecret;
                options.Authority = configuration.Authority;
                options.MetadataAddress = configuration.MetaDataUrl;
                options.CallbackPath = new PathString(configuration.CallbackUrl);
                options.SignedOutRedirectUri = new PathString(configuration.SignoutRedirectUrl);
                options.SignedOutCallbackPath = new PathString(configuration.SignoutCallbackUrl);
                options.ResponseType = OpenIdConnectResponseType.Code;

                options.Scope.Clear();
                foreach (string scope in configuration.Scopes) {
                    options.Scope.Add(scope);
                }

                options.GetClaimsFromUserInfoEndpoint = configuration.GetClaimsFromUserInfoEndpoint;
                options.SaveTokens = configuration.SaveTokens;

                options.Events = new OpenIdConnectEvents() {
                    OnTokenValidated = async (context) => {
                        if (configuration.DiscoverRolesWithPublicApi) {
                            await AddRoleClaimsFromDfePublicApi(context);
                        }
                    }
                };
            })
            .AddCookie(options => {
                options.Cookie.Name = configuration.CookieName;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(configuration.CookieExpiration);
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.SlidingExpiration = configuration.SlidingExpiration;
            });
    }

    private static async Task AddRoleClaimsFromDfePublicApi(TokenValidatedContext context)
    {
        var dfePublicApi = context.HttpContext.RequestServices.GetRequiredService<IDfePublicApi>();

        if (context.Principal?.Identity?.IsAuthenticated == true) {
            var claims = context.Principal.Claims;
            var userOrganization = JsonHelpers.Deserialize<Organisation>(
                claims
                    .Where(c => c.Type == ClaimConstants.Organisation)
                    .Select(c => c.Value)
                    .FirstOrDefault("null")
            );

            if (userOrganization == null) {
                context.Fail("User is not in an organisation.");
                return;
            }

            var userId = claims
                .Where(c => c.Type.Contains(ClaimConstants.NameIdentifier))
                .Select(c => c.Value)
                .Single();

            var userAccessToService = await dfePublicApi.GetUserAccessToService(userId, userOrganization.Id.ToString());
            var roleClaims = new List<Claim>();
            foreach (var role in userAccessToService.Roles) {
                if (role.Status.Id.Equals(1)) {
                    roleClaims.Add(new Claim(ClaimConstants.RoleCode, role.Code, ClaimTypes.Role, context.Options.ClientId));
                    roleClaims.Add(new Claim(ClaimConstants.RoleId, role.Id.ToString(), ClaimTypes.Role, context.Options.ClientId));
                    roleClaims.Add(new Claim(ClaimConstants.RoleName, role.Name, ClaimTypes.Role, context.Options.ClientId));
                    roleClaims.Add(new Claim(ClaimConstants.RoleNumericId, role.NumericId.ToString(), ClaimTypes.Role, context.Options.ClientId));
                }
            }

            var roleIdentity = new ClaimsIdentity(roleClaims);
            context.Principal.AddIdentity(roleIdentity);
        }
    }
}
