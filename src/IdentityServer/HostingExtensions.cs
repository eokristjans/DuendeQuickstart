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
