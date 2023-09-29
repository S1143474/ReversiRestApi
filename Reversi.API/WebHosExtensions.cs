using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Reversi.API.Infrastructure.Persistence;

namespace Reversi.API
{
    public static class WebHosExtensions
    {
        public static IHost SeedData(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.GetRequiredService<RepositoryContext>();

                    var seeder = new DbInitializer(context);

                    seeder.Seed();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();

                    logger.LogCritical($"An error occurred initializing the database: {ex}");
                }
            }

            return host;
        }
    }
}
