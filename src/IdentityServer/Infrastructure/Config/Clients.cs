using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Clients
{
    /// <summary>
    /// In quickstart 1, the machine-to-machine client will not have an interactive user
    /// and will authenticate with IdentityServer using a client secret.
    /// </summary>
    /// <returns>A client with complete ApiAccess, authenticated with clientId/secret</returns>
    public static Client GetMachineToMachineClient()
    {
        return new Client
        {
            ClientId = "client",

            // no interactive user, use the clientId/secret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            // secret for authentication (should generally be a real secret)
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },

            // scopes that client has access to
            AllowedScopes = { Constants.ApiScopes.API1 }
        };
    }

    public static Client GetInteractiveClient()
    {
        return new Client
        {
            ClientId = "web",

            // secret for authentication (should generally be a real secret)
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedGrantTypes = GrantTypes.Code,

            // where to redirect to after login
            RedirectUris = { "https://localhost:5002/signin-oidc" },

            // where to redirect to after logout
            PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

            // scopes that client has access to
            AllowedScopes = new List<string>
            {
                // Identity Resources, part of the identity token,
                // containing information about the authentication process and session
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                Constants.IdentityResourceScopes.Verification,

                // Add ApiScope that gives access to the API, as part of the Access Token
                Constants.ApiScopes.API1
            }
        };
    }
}