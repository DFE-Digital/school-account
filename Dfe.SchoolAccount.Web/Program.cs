using Dfe.SchoolAccount.SignIn;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RequestLocalizationOptions>(options => {
    var supportedCultures = new[] { "en" /*, "cy"*/ };

    options.SetDefaultCulture(supportedCultures[0]);
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
    options.FallBackToParentUICultures = true;
    options.ApplyCurrentCultureToResponseHeaders = true;
});

//var dfePublicApiConfiguration = new DfePublicApiConfiguration();
//builder.Configuration.GetSection("DfePublicApi").Bind(dfePublicApiConfiguration);
//builder.Services.AddDfeSignInPublicApi(dfePublicApiConfiguration);

var dfeSignInConfiguration = new DfeSignInConfiguration();
builder.Configuration.GetSection("DfeSignIn").Bind(dfeSignInConfiguration);
builder.Services.AddDfeSignInAuthentication(dfeSignInConfiguration);

//Sample to add authorisation to restrict user access to service based on a claim value
//services.AddAuthorization(options =>
//{
//    options.AddPolicy("#policy_name#",
//         policy => policy.RequireClaim("#claim_name#", "#claim_value#"));
//});

// Add services to the container.
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
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();
