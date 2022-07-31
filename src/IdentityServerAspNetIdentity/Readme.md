# IdentityServerAspNetIdentity

[Based on Duende v6 Quickstart 5](https://docs.duendesoftware.com/identityserver/v6/quickstarts/5_aspnetid/).

Initiated with `dotnet new isaspid -n IdentityServerAspNetIdentity`.

Added scopes and clients from Quickstart 1-3 so that this IdentityServer can replace the 
IdentityServer we built previously. The Client can authenticate with this IS to get access
to the Api. A user can interactively authenticate with this IS to get access to the Api and 
to authorize the WebClient to call the Api on the user's behalf.

## Task 1

Move 

In Quickstart 4 we moved configuration and other temporary data into an Sqlite database 
using Entity Framework, but the template used to initiate this server still uses InMemoryStores. 
Can follow [Quickstart 4](https://docs.duendesoftware.com/identityserver/v6/quickstarts/4_ef/) 
again to add Sqlite.

## Tasks 2-6

At the [end of Quickstart 3](https://docs.duendesoftware.com/identityserver/v6/quickstarts/3_api_access/#further-reading---access-token-lifetime-management) 
they suggested the following tasks, all of which are important additions.

By far the most complex task for a typical client is to manage the access token. 
You typically want to:

* request the access and refresh token at login time
* cache those tokens
* use the access token to call APIs until it expires
* use the refresh token to get a new access token
* repeat the process of caching and refreshing with the new token

ASP.NET Core has built-in facilities that can help you with some of those tasks (like 
caching or sessions), but there is still quite some work left to do. Consider using 
the [IdentityModel](https://identitymodel.readthedocs.io/en/latest/aspnetcore/overview.html) 
library for help with access token lifetime management. It provides 
abstractions for storing tokens, automatic refresh of expired tokens, etc.

## Tasks 7-8

At the [end of Quickstart 5](https://docs.duendesoftware.com/identityserver/v6/quickstarts/5_aspnetid/#whats-missing)
they suggested adding UI code for user registration, password reset, and other things 
you might expect from Microsoft’s templates that include 
[ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-6.0&tabs=netcore-cli#create-a-web-app-with-authentication).

## Task 9

Connect to Azure KeyVault and use it for client secrets and keys.

## Task 10 

Add Application Insights.

## Task 11

Consider replacing SQLite with Postgres.

## Task 12

Deploy to Azure cloud.