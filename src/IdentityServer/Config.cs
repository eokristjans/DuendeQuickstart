using Duende.IdentityServer.Models;

namespace IdentityServer;

/// <summary>
/// Code as configuration
/// </summary>
public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId()
        };

    /// <summary>
    /// Scope is a core feature of OAuth that allows you to express the extent or scope of access.
    /// OAuth 2.0 scopes provide a way to limit the amount of access that is granted to an access token.
    /// 
    /// Scope is a mechanism in OAuth 2.0 to limit an application's access to a user's account. 
    /// An application can request one or more scopes, this information is then presented to the user in 
    /// the consent screen, and the access token issued to the application will be limited to the scopes granted.
    /// 
    /// Ultimately, IdentityServer issues a token to the client, which then uses the token to access APIs. 
    /// APIs can check the scopes that were included in the token to make authorization decisions.
    /// </summary>
    public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
    // In production it is important to give your API a useful name and display name.
    // Use these names to describe your API in simple terms to both developers and users.
    // Developers will use the name to connect to your API,
    // and end users will see the display name on consent screens, etc.
    {
        // Create a scope that represents complete access to the API.
        new ApiScope(name: "api1", displayName: "MyAPI") 
    };

    public static IEnumerable<Client> Clients => new Client[] 
    {
        // In this quickstart, the machine-to-machine client will not have an interactive user
        // and will authenticate with IdentityServer using a client secret.
        new Client
        {
            ClientId = "client",

            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            // secret for authentication (should generally be a real secret)
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },

            // scopes that client has access to
            AllowedScopes = { "api1" }
        }
    };
}