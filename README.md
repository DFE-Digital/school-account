# school-account

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=DFE-Digital_school-account&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=DFE-Digital_school-account)

## Setting up secrets locally

Use the [dotnet user-secrets set](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-7.0&tabs=windows#set-a-secret) command to set secrets locally.

For example,

```bash
dotnet user-secrets set "DfePublicApi:ClientId" "<client_id>"
dotnet user-secrets set "DfePublicApi:ApiSecret" "<api_secret>"
dotnet user-secrets set "DfeSignIn:ClientId" "<client_id>"
dotnet user-secrets set "DfeSignIn:ClientSecret" "<client_secret>"
```

> **DO NOT** place secrets into any appsettings.json file inside the project since they may be inadvertently committed to the repository.


## Configuring Contentful space secrets

The application will need access to a Contentful space to be able to retrieve information to display to the user. You may error out once you log in through DfE Sign-in without these secrets set.

Once you have a Space ID, a Delivery API key and a Preview API key, you can set them as follows:

```bash
dotnet user-secrets set "ContentfulOptions:SpaceId" "<space_id>"
dotnet user-secrets set "ContentfulOptions:DeliveryApiKey" "<delivery_api_key>"
dotnet user-secrets set "ContentfulOptions:PreviewApiKey" "<preview_api_key>"
```

## Restricting access to users of permitted organisations

Service access can be restricted to users of permitted organisations by providing a list of organisation GUID's. Access is not restricted when this configuration is not provided.

For example, to specify the GUID of multiple organisations:

```bash
dotnet user-secrets set "RestrictedAccess:PermittedOrganisationIds:0" "<organisation_guid>"
dotnet user-secrets set "RestrictedAccess:PermittedOrganisationIds:1" "<organisation_guid>"
dotnet user-secrets set "RestrictedAccess:PermittedOrganisationIds:2" "<organisation_guid>"
```

## Run unit tests

Unit tests can be run within Visual Studio via the "Test -> Run All Tests" menu.

Tests can also be run with the following command from the solution directory:

```bash
dotnet test
```

## Build frontend scripts and styles

Run the command `npm run build` to build the .js and .css frontend files.

When developing scripts and styles it is useful to use the `npm run build:watch` command so that the .js and .css frontend files are automatically rebuilt.

> The built frontend files are output to the `Dfe.SchoolAccount.Web/wwwroot/` directory and are not committed to the repository.
