using Azure.Identity;
using Contentful.AspNetCore;
using Contentful.Core.Configuration;
using Contentful.Core;
using Dfe.SchoolAccount.SignIn;
using Dfe.SchoolAccount.Web.Authorization;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.ContentTransformers;
using Dfe.SchoolAccount.Web.Services.ContentTransformers.Cards;
using Dfe.SchoolAccount.Web.Services.Personas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

// Limit execution of regular expressions (Category: DoS).
AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(200));

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("DFE_SA_");

var keyVaultName = builder.Configuration["KeyVaultName"];
if (!string.IsNullOrEmpty(keyVaultName) && keyVaultName != "<key_vault_name>") {
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{keyVaultName}.vault.azure.net/"),
        new DefaultAzureCredential()
    );
}

builder.Services.Configure<RequestLocalizationOptions>(options => {
    var supportedCultures = new[] { "en" /*, "cy"*/ };

    options.SetDefaultCulture(supportedCultures[0]);
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
    options.FallBackToParentUICultures = true;
    options.ApplyCurrentCultureToResponseHeaders = true;
});

//// Configure `DiscoverRolesWithPublicApi` as `true` in 'appsettings.json' to enable.
//var dfePublicApiConfiguration = new DfePublicApiConfiguration();
//builder.Configuration.GetSection("DfePublicApi").Bind(dfePublicApiConfiguration);
//builder.Services.AddDfeSignInPublicApi(dfePublicApiConfiguration);

var dfeSignInConfiguration = new DfeSignInConfiguration();
builder.Configuration.GetSection("DfeSignIn").Bind(dfeSignInConfiguration);
builder.Services.AddDfeSignInAuthentication(dfeSignInConfiguration);

var restrictedAccessSection = builder.Configuration.GetSection("RestrictedAccess");
if (restrictedAccessSection.Exists()) {
    var restrictedAccessConfiguration = new RestrictedAccessConfiguration();
    builder.Configuration.GetSection("RestrictedAccess").Bind(restrictedAccessConfiguration);
    builder.Services.AddSingleton<IRestrictedAccessConfiguration>(restrictedAccessConfiguration);
    builder.Services.AddSingleton<IAuthorizationHandler, RestrictedAccessAuthorizationHandler>();
}

builder.Services.AddSingleton<IAuthorizationHandler, RestrictToSchoolUsersAuthorizationHandler>();

//Sample to add authorisation to restrict user access to service based on a claim value
//services.AddAuthorization(options =>
//{
//    options.AddPolicy("#policy_name#",
//         policy => policy.RequireClaim("#claim_name#", "#claim_value#"));
//});

// Override registration of `IContentfulClient` so that `ContentTypeResolver` can be provided.
builder.Services.AddTransient<IContentfulClient>((IServiceProvider sp) =>
{
    ContentfulOptions contentfulOptions = sp.GetService<IOptions<ContentfulOptions>>()!.Value;
    IHttpClientFactory httpClientFactory = sp.GetService<IHttpClientFactory>()!;
    var contentfulClient = new ContentfulClient(httpClientFactory.CreateClient("ContentfulClient"), contentfulOptions);
    contentfulClient.ContentTypeResolver = new CustomContentTypeResolver();
    return contentfulClient;
});
builder.Services.AddContentful(builder.Configuration);

builder.Services.AddSingleton<IPersonaResolver, OrganisationTypePersonaResolver>();
builder.Services.AddSingleton<IHubContentFetcher, ContentfulHubContentFetcher>();

builder.Services.AddSingleton<IContentViewModelTransformHandler<ExternalResourceContent, CardModel>, ExternalResourceContentToCardTransformHandler>();
builder.Services.AddSingleton<IContentViewModelTransformHandler<SignpostingPageContent, CardModel>, SignpostingPageContentToCardTransformHandler>();
builder.Services.AddSingleton<IContentViewModelTransformer, RegisteredServicesContentViewModelTransformer>();

builder.Services.AddControllersWithViews()
    .AddMvcLocalization(options => {
        options.ResourcesPath = "Resources";
    });

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    _ = app.UseExceptionHandler("/error?statusCode=500");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _ = app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRequestLocalization();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", context => {
    context.Response.Redirect("/start", permanent: true);
    return Task.CompletedTask;
});

app.MapControllerRoute(
    name: "restricted",
    pattern: "restricted",
    defaults: new { controller = "Error", action = "Index", statusCode = 403 }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
);

app.Run();
