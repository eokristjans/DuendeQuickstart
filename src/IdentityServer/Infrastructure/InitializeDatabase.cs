using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer
{
    public static class InitializeDatabase
    {
        /// <summary>
        /// dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
        /// dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb
        /// 
        /// The following code creates the database from the migrations above (which we ran) and 
        /// seeds the database with the same configuration data used in the previous quickstarts.
        /// 
        /// The approach used in this quickstart is used to make it easy to get IdentityServer up and running. 
        /// You should devise your own database creation and maintenance strategy that is appropriate for your architecture.
        /// </summary>
        public static void CreateAndSeedFromMigrationsAndQuickstartConfigData(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}