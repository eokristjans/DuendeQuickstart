using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServerAspNetIdentity;

/// <summary>
/// Code as configuration
/// </summary>
public static class Config
{
    /// <summary>
    /// An identity resource is a named group of claims about a user that 
    /// can be requested using the scope parameter.
    /// 
    /// Similar to OAuth, OpenID Connect uses scopes to represent something 
    /// you want to protect and that clients want to access. In contrast to OAuth, 
    /// scopes in OIDC represent identity data like 
    /// user id, name or email address rather than APIs.
    /// </summary>
    public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[]
    {
        new IdentityResources.OpenId(),// SubjectId
        
        // All standard scopes and their corresponding claims can be found in the
        // OpenID Connect specification.
        // https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims
        // OPTIONAL. This scope value requests access to the End-User's default
        // profile Claims, which are: name, family_name, given_name, preferred_username,
        // picture, website, gender, birthdate etc.
        // Other OIDC-defined optional claims are email, address and phone.
        // These scope values are returned from the UserInfo Endpoint (/connect/userinfo).
        new IdentityResources.Profile(),

        // Quickstart 2 Further Experiments. Add an extra claim and then give the client access to it.
        new IdentityResource()
        {
            // The Name property of the resource is the scope value that clients
            // can request to get the associated UserClaims. 
            // Hence probably wise to define the scope name as a constant.
            Name = "verification",
            UserClaims = new List<string>
            {
                JwtClaimTypes.Email,
                JwtClaimTypes.EmailVerified
            }
        }
    };

    /// <summary>
    /// Scope is a core feature of OAuth that allows you to express the extent or scope 
    /// of access. OAuth 2.0 scopes provide a way to limit the amount of access that is 
    /// granted to an access token.
    /// 
    /// Scope is a mechanism in OAuth 2.0 to limit an application's access to a user's account. 
    /// An application can request one or more scopes, this information is then presented to 
    /// the user in the consent screen, and the access token issued to the application will 
    /// be limited to the scopes granted.
    /// 
    /// Ultimately, IdentityServer issues a token to the client, which then uses the token 
    /// to access APIs. APIs can check the scopes that were included in the token to make 
    /// authorization decisions.
    /// 
    /// In production it is important to give your API a useful name and display name.
    /// Use these names to describe your API in simple terms to both developers and users.
    /// Developers will use the name to connect to your API,
    /// and end users will see the display name on consent screens, etc.
    /// </summary>
    public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
    {
        new ApiScope("api1", "My API") 
        {
            Description = "Represents complete access to the API" 
        },
        new ApiScope("scope1"), // (from isaspid template)
        new ApiScope("scope2"), // (from isaspid template)
    };

    public static IEnumerable<Client> Clients => new Client[]
    {
        // machine to machine client
        new Client
        {
            ClientId = "client",
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedGrantTypes = GrantTypes.ClientCredentials,
            // scopes that client has access to
            AllowedScopes = { "api1" }
        },
                
        // interactive ASP.NET Core Web App
        new Client
        {
            ClientId = "web",
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedGrantTypes = GrantTypes.Code,
                    
            // where to redirect to after login
            RedirectUris = { "https://localhost:5002/signin-oidc" },

            // where to redirect to after logout
            PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

            AllowOfflineAccess = true,

            AllowedScopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "verification",
                "api1"
            }
        },

        // m2m client credentials flow client (from isaspid template)
        new Client
        {
            ClientId = "m2m.client",
            ClientName = "Client Credentials Client",

            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

            AllowedScopes = { "scope1" }
        },

        // interactive client using code flow + pkce (from isaspid template)
        new Client
        {
            ClientId = "interactive",
            ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

            AllowedGrantTypes = GrantTypes.Code,

            RedirectUris = { "https://localhost:44300/signin-oidc" },
            FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
            PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

            AllowOfflineAccess = true,
            // Could use IdentityServerConstants.StandardScopes.OpenId & ..Profile
            AllowedScopes = { "openid", "profile", "scope2" }
        },
    };
}
