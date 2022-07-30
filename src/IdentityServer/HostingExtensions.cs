using Duende.IdentityServer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Quickstart 2 (add UI)
        builder.Services.AddRazorPages();

        builder.Services.AddIdentityServer(options =>
            {
                // https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/api_scopes#authorization-based-on-scopes
                options.EmitStaticAudienceClaim = true;
            })
            // Added just for kicks after Quickstart 2.
            // Enables a page where an authenticated user can view and delete his sessions.
            .AddServerSideSessions() 

            // Load the scopes and clients from Config.cs, although these extension methods also
            // support adding Resources, ApiScopes and Clients from the ASP.NET Core configuration file
            // https://docs.duendesoftware.com/identityserver/v6/fundamentals/clients/#defining-clients-in-appsettingsjson
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddTestUsers(TestUsers.Users); // Quickstart 2

        builder.Services.AddAuthentication()

            // Adding Google as an external authenticaiton is supposedly very easy, but requires 
            // installing a nuget package, registering with Google and setting up a client.
            // https://docs.duendesoftware.com/identityserver/v6/quickstarts/2_interactive/#add-google-support
            /*
            .AddGoogle("Google", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            });
            */
            // Adding this additional Demo OpenID Connect-based external provider does not require any registration.
            // There one can log in as Bob, or log in with Google to the Demo IdentityServer. Google then
            // redirects you back to the Demo IdentityServer which redirects you back to your own page.
            // But these IdentityServers have not given us access to claims, it seems.
            .AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                options.SaveTokens = true;

                options.Authority = "https://demo.duendesoftware.com";
                options.ClientId = "interactive.confidential";
                options.ClientSecret = "secret";
                options.ResponseType = "code";

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
            });

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    { 
        app.UseSerilogRequestLogging();
    
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Quickstart 2 (add UI)
        app.UseStaticFiles();
        app.UseRouting();
            
        app.UseIdentityServer();

        // Quickstart 2 (add UI)
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        return app;
    }
}
