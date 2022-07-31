using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddAuthentication(options =>
{
    // The DefaultChallengeScheme is used when an unauthenticated user must log in (enters the page).
    // This begins the OpenID Connect protocol, redirecting the user to IdentityServer.
    // After the user has logged in and been redirected back to the client,
    // the client creates its own local cookie. Subsequent requests to the client
    // will include this cookie and be authenticated with the default Cookie scheme.
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    // After the call to AddAuthentication, AddCookie adds the handler that can process the local cookie.
    .AddCookie("Cookies")

    // Finally, AddOpenIdConnect is used to configure the handler that performs the OpenID Connect protocol.
    .AddOpenIdConnect("oidc", options =>
    {
        // The Authority indicates where the trusted token service is located.
        options.Authority = "https://localhost:5001";

        // The ClientId and the ClientSecret identify this client.
        options.ClientId = "web";
        options.ClientSecret = "secret";
        options.ResponseType = "code";

        // SaveTokens is used to persist the tokens in the cookie (as they will be needed later).
        // Since SaveTokens is enabled, ASP.NET Core will automatically store the id, access,
        // and refresh tokens (!) in the properties of the authentication cookie.
        options.SaveTokens = true;

        // The Scope is the collection of scopes that the client will request.
        // By default it includes the openid and profile scopes, but clear the collection
        // and add them back for explicit clarity.
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        
        // Quickstart 2 Further Experiments:
        // Requesting the scope that was added and access granted to this client.
        options.Scope.Add("verification");
        // JwtClaimTypes.EmailVerified exists as a constant on IdentityModel and is specified on the
        // TestUsers in the IdentityServer, which are mapped by TestUserProfileService: IProfileService.
        // You can provide your own implementation of IProfileService to customize
        // this process with custom logic, data access, etc.
        options.ClaimActions.MapJsonKey("email_verified", "email_verified");

        // Quickstart 3: Request ApiScope with access to all of api1 on behalf of the authenticated user
        options.Scope.Add("api1");
        //options.Scope.Add("offline_access");

        // The IS is configured so that the WebClient can request the claims associated with
        // the profile scope, but we must tell the client to retrieve them from the userinfo endpoint.
        options.GetClaimsFromUserInfoEndpoint = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for
    // production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages().RequireAuthorization();

app.Run();
