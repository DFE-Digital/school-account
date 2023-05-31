using Azure.Identity;
using Contentful.AspNetCore;
using Contentful.Core;
using Contentful.Core.Configuration;
using Contentful.Core.Models;
using Dfe.SchoolAccount.SignIn;
using Dfe.SchoolAccount.Web.Authorization;
using Dfe.SchoolAccount.Web.Configuration;
using Dfe.SchoolAccount.Web.Constants;
using Dfe.SchoolAccount.Web.Models.Content;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks;
using Dfe.SchoolAccount.Web.Services.ContentHyperlinks.Handlers;
using Dfe.SchoolAccount.Web.Services.ContentTransformers;
using Dfe.SchoolAccount.Web.Services.ContentTransformers.Cards;
using Dfe.SchoolAccount.Web.Services.Personas;
using Dfe.SchoolAccount.Web.Services.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler>((IServiceProvider sp) => {
    var logger = sp.GetRequiredService<ILogger<FailedAuthorizationMiddlewareResultHandler>>();
    var defaultHandler = new AuthorizationMiddlewareResultHandler();
    return new FailedAuthorizationMiddlewareResultHandler(logger, defaultHandler);
});

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
builder.Services.AddTransient((IServiceProvider sp) => {
    var renderer = new HtmlRenderer();
    renderer.AddRenderer(new CustomHeadingRenderer(renderer.Renderers));
    renderer.AddRenderer(new CustomListContentRenderer(renderer.Renderers));
    renderer.AddRenderer(new CustomQuoteRenderer(renderer.Renderers));
    renderer.AddRenderer(new CardGroupModelRenderer(sp.GetRequiredService<IRazorViewEngine>(), sp.GetRequiredService<ITempDataProvider>(), sp) { Order = 10 });
    renderer.AddRenderer(new ContentLinkRenderer(sp.GetRequiredService<IContentHyperlinkResolver>(), renderer.Renderers));
    return renderer;
});

builder.Services.AddSingleton<IPersonaResolver, OrganisationTypePersonaResolver>();
builder.Services.AddSingleton<IHubContentFetcher, ContentfulHubContentFetcher>();
builder.Services.AddSingleton<ISignpostingPageContentFetcher, ContentfulSignpostingPageContentFetcher>();
builder.Services.AddSingleton<IErrorPageContentFetcher, ContentfulErrorPageContentFetcher>();
builder.Services.AddSingleton<IPageContentFetcher, ContentfulPageContentFetcher>();
builder.Services.AddSingleton<IWebsiteGlobalsContentFetcher, ContentfulWebsiteGlobalsContentFetcher>();

builder.Services.Configure<EntryCacheOptions>(EntryCacheConstants.WebsiteGlobalsContent, builder.Configuration.GetSection("WebsiteGlobalsContent:MemoryCache"));
builder.Services.Decorate<IWebsiteGlobalsContentFetcher, MemoryCacheWebsiteGlobalsContentFetcherDecorator>();

builder.Services.AddSingleton<IContentModelTransformHandler<ExternalResourceContent, CardModel>, ExternalResourceContentToCardTransformHandler>();
builder.Services.AddSingleton<IContentModelTransformHandler<SignpostingPageContent, CardModel>, SignpostingPageContentToCardTransformHandler>();
builder.Services.AddSingleton<IContentModelTransformer, RegisteredServicesContentModelTransformer>();

builder.Services.AddSingleton<IContentHyperlinkResolutionHandler<PageContent>, PageContentHyperlinkResolutionHandler>();
builder.Services.AddSingleton<IContentHyperlinkResolutionHandler<ExternalResourceContent>, ExternalResourceContentHyperlinkResolutionHandler>();
builder.Services.AddSingleton<IContentHyperlinkResolutionHandler<SignpostingPageContent>, SignpostingPageContentHyperlinkResolutionHandler>();
builder.Services.AddSingleton<IContentHyperlinkResolver, RegisteredServicesContentHyperlinkResolver>();

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
    name: ErrorPageConstants.YourInstitutionIsNotYetEligibleForThisServiceHandle,
    pattern: ErrorPageConstants.YourInstitutionIsNotYetEligibleForThisServiceHandle,
    defaults: new { controller = "ErrorPage", action = "Index", handle = ErrorPageConstants.YourInstitutionIsNotYetEligibleForThisServiceHandle, statusCode = 403 }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "page",
    pattern: "{slug}",
    defaults: new { controller = "Page", action = "Index" }
);

app.Run();
