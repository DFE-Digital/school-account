using Azure.Identity;
using Contentful.AspNetCore;
using Dfe.SchoolAccount.SignIn;
using Dfe.SchoolAccount.Web.Authorization;
using Dfe.SchoolAccount.Web.Services.Content;
using Dfe.SchoolAccount.Web.Services.Personas;
using Microsoft.AspNetCore.Authorization;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables("DFE_SA_");

if (!string.IsNullOrEmpty(builder.Configuration["KeyVaultName"])) {
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
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

builder.Services.AddContentful(builder.Configuration);

builder.Services.AddSingleton<IPersonaResolver, OrganisationTypePersonaResolver>();
builder.Services.AddSingleton<IHubContentFetcher, ContentfulHubContentFetcher>();

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
