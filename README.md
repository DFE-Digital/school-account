# school-account

## Setting up secrets locally

Use the [dotnet user-secrets set](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows#set-a-secret) command to set secrets locally.

For example,

```bash
dotnet user-secrets set "DfePublicApi:ClientId" "THE CLIENT ID"
dotnet user-secrets set "DfePublicApi:ApiSecret" "THE API SECRET"
dotnet user-secrets set "DfeSignIn:ClientId" "THE CLIENT ID"
dotnet user-secrets set "DfeSignIn:ClientSecret" "THE CLIENT SECRET"
```

> **DO NOT** place secrets into any appsettings.json file inside the project since they may be inadvertently committed to the repository.

## Build frontend scripts and styles

Run the command `npm run build` to build the .js and .css frontend files.

When developing scripts and styles it is useful to use the `npm run build:watch` command so that the .js and .css frontend files are automatically rebuilt.

> The built frontend files are output to the `Dfe.SchoolAccount.Web/wwwroot/` directory and are not committed to the repository.
