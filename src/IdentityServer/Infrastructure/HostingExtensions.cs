using Duende.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace IdentityServer;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        // Quickstart 2 (add UI)
        builder.Services.AddRazorPages();

        // Quickstart 4: Configure Sqlite DB for configuration and operational data store
        var migrationsAssembly = typeof(Program).Assembly.GetName().Name;
        const string connectionString = @"Data Source=Duende.IdentityServer.Quickstart.EntityFramework.db";

        // You will use Entity Framework migrations later on in this quickstart to manage the database schema.
        // The call to MigrationsAssembly(...) tells Entity Framework that the host project will contain the migrations.
        // This is necessary since the host project is in a different assembly than the one that contains the DbContext classes.
        builder.Services.AddIdentityServer()
            .AddConfigurationStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlite(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = b => b.UseSqlite(connectionString,
                    sql => sql.MigrationsAssembly(migrationsAssembly));
            })
            .AddTestUsers(TestUsers.Users); // Quickstart 2

        builder.Services.AddAuthentication()
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

        // Now if you run the IdentityServer project, the database should be created
        // and seeded with the quickstart configuration data.
        // You should be able to use a tool like SQL Lite Studio to connect and inspect the data.
        // It should be unnecessary and perhaps even devastating to run this again.
        // Once the database has been populated, this can be commented out.
        //InitializeDatabase.CreateAndSeedFromMigrationsAndQuickstartConfigData(app);

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
