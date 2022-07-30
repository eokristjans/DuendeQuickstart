using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServer;

/// <summary>
/// Code as configuration
/// </summary>
public static class Config
{

    /// <summary>
    /// Quickstart 2.
    /// An identity resource is a named group of claims about a user that can be requested using the scope parameter.
    /// 
    /// Similar to OAuth, OpenID Connect uses scopes to represent something you want to protect 
    /// and that clients want to access. In contrast to OAuth, scopes in OIDC represent identity 
    /// data like user id, name or email address rather than APIs.
    /// </summary>
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(), // SubjectId

            // All standard scopes and their corresponding claims can be found in OpenID Connect
            // specification. https://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims
            // OPTIONAL. This scope value requests access to the End-User's default profile Claims,
            // which are: name, family_name, given_name, preferred_username, picture, website, gender, birthdate etc.
            // Other OIDC-defined optional claims are email, address and phone.
            // These scope values are returned from the UserInfo Endpoint (/connect/userinfo).
            new IdentityResources.Profile()
        };

    public static IEnumerable<Client> Clients => new Client[]
    {
        IdentityServer.Clients.GetMachineToMachineClient(),
        IdentityServer.Clients.GetInteractiveClient()
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
    /// 
    /// In production it is important to give your API a useful name and display name.
    /// Use these names to describe your API in simple terms to both developers and users.
    /// Developers will use the name to connect to your API,
    /// and end users will see the display name on consent screens, etc.
    /// </summary>
    public static IEnumerable<ApiScope> ApiScopes => new ApiScope[]
    {
        GetScopeWithCompleteAccessToApi()
    };

    /// <summary> Quickstart 1 </summary>
    /// <returns>A scope that represents complete access to the API.</returns>
    private static ApiScope GetScopeWithCompleteAccessToApi()
    {
        return new ApiScope(name: "api1", displayName: "MyAPI");
    }
}