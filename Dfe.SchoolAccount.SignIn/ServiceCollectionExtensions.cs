namespace Dfe.SchoolAccount.SignIn;

using System.Net;
using System.Security.Claims;
using System.Text.Json;
using Dfe.SchoolAccount.SignIn.Constants;
using Dfe.SchoolAccount.SignIn.DsiClient;
using Dfe.SchoolAccount.SignIn.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

public static class ServiceCollectionExtensions
{
    public static void AddDfESignInAuthentication(this IServiceCollection services, DfESignInConfig dfeSignInConfiguration)
    {
        services.AddHttpClient();
        services.AddHttpClient<DfESignInClient>()
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler {
                AllowAutoRedirect = true,
                UseDefaultCredentials = false,
                PreAuthenticate = true,
                Proxy = string.IsNullOrWhiteSpace(dfeSignInConfiguration.APIServiceProxyUrl) ? new WebProxy() : new WebProxy(new Uri(dfeSignInConfiguration.APIServiceProxyUrl, UriKind.Absolute))
            });
        services.AddHttpContextAccessor();
        services.AddAuthentication(sharedOptions => {
            sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
       .AddOpenIdConnect(options => {
           options.ClientId = dfeSignInConfiguration.ClientId;
           options.ClientSecret = dfeSignInConfiguration.ClientSecret;
           options.Authority = dfeSignInConfiguration.Authority;
           options.MetadataAddress = dfeSignInConfiguration.MetaDataUrl;
           options.CallbackPath = new PathString(dfeSignInConfiguration.CallbackUrl);
           options.SignedOutRedirectUri = new PathString(dfeSignInConfiguration.SignoutRedirectUrl);
           options.SignedOutCallbackPath = new PathString(dfeSignInConfiguration.SignoutCallbackUrl);
           options.ResponseType = OpenIdConnectResponseType.Code;

           options.Scope.Clear();
           foreach (string scope in dfeSignInConfiguration.Scopes) {
               options.Scope.Add(scope);
           }

           options.GetClaimsFromUserInfoEndpoint = dfeSignInConfiguration.GetClaimsFromUserInfoEndpoint;
           options.SaveTokens = dfeSignInConfiguration.SaveTokens;

           //API call demonstration code from Startup by extending options to include OpenIfConnectEvents
           //Get role details from claims
           options.Events = new OpenIdConnectEvents() {
               OnTokenValidated = async (tokenValidatedContext) => {
                   if (tokenValidatedContext.Principal != null && tokenValidatedContext.Principal.Identity != null) {
                       if (tokenValidatedContext.Principal.Identity.IsAuthenticated) {
                           var claims = tokenValidatedContext.Principal.Claims;
                           var userOrganization = JsonSerializer.Deserialize<Organisation>
                               (
                                   claims
                                       .Where(c => c.Type == ClaimConstants.Organisation)
                                       .Select(c => c.Value)
                                       .FirstOrDefault("null")
                               );

                           var clientFactory = new DfESignInClientFactory(dfeSignInConfiguration);
                           var userId = claims
                               .Where(c => c.Type.Contains(ClaimConstants.NameIdentifier))
                               .Select(c => c.Value)
                               .SingleOrDefault();

                           if (userId == null || userOrganization == null) {
                               throw new Exception("Missing user details.");
                           }

                           var dfeSignInClient = clientFactory.CreateDfESignInClient(userId, userOrganization.Id.ToString());

                           var res = await dfeSignInClient.HttpClient.GetAsync(dfeSignInClient.TargetAddress);

                           if (res.IsSuccessStatusCode) {
                               var resMessage = await res.Content.ReadAsStringAsync();

                               var apiServiceResponse = JsonSerializer.Deserialize<ApiServiceResponse>(resMessage);
                               if (apiServiceResponse == null) {
                                   throw new Exception("Invalid response.");
                               }

                               var roleClaims = new List<Claim>();
                               foreach (var role in apiServiceResponse.Roles) {
                                   if (role.Status.Id.Equals(1)) {
                                       roleClaims.Add(new Claim(ClaimConstants.RoleCode, role.Code, ClaimTypes.Role, tokenValidatedContext.Options.ClientId));
                                       roleClaims.Add(new Claim(ClaimConstants.RoleId, role.Id.ToString(), ClaimTypes.Role, tokenValidatedContext.Options.ClientId));
                                       roleClaims.Add(new Claim(ClaimConstants.RoleName, role.Name, ClaimTypes.Role, tokenValidatedContext.Options.ClientId));
                                       roleClaims.Add(new Claim(ClaimConstants.RoleNumericId, role.NumericId.ToString(), ClaimTypes.Role, tokenValidatedContext.Options.ClientId));
                                   }
                               }

                               var roleIdentity = new ClaimsIdentity(roleClaims);
                               tokenValidatedContext.Principal.AddIdentity(roleIdentity);
                           }
                       }
                   }
               }
           };
       })
       .AddCookie(options => {
           options.Cookie.Name = dfeSignInConfiguration.CookieName;
           options.ExpireTimeSpan = TimeSpan.FromMinutes(dfeSignInConfiguration.CookieExpiration);
           options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
           options.SlidingExpiration = dfeSignInConfiguration.SlidingExpiration;
       });

        //Sample to add authorisation to restrict user access to service based on a claim value
        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("#policy_name#",
        //         policy => policy.RequireClaim("#claim_name#", "#claim_value#"));
        //});
    }
}
