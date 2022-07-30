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

        // The Scope is the collection of scopes that the client will request.
        // By default it includes the openid and profile scopes, but clear the collection
        // and add them back for explicit clarity.
        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");

        // The IS is configured so that the WebClient can request the claims associated with
        // the profile scope, but we must tell the client to retrieve them from the userinfo endpoint.
        options.GetClaimsFromUserInfoEndpoint = true;

        // SaveTokens is used to persist the tokens in the cookie (as they will be needed later).
        options.SaveTokens = true;
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
