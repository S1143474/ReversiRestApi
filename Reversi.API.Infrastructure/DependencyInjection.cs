using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reversi.API.Application.Common;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Infrastructure.Persistence;
using Reversi.API.Infrastructure.Repository;
using Reversi.API.Infrastructure.Services;

namespace Reversi.API.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<ISpelRepository, SpelAccessLayer>();
            var connectionString = configuration["sqlconnection:connectionString"];

            services.AddDbContext<RepositoryContext>(options => options.UseSqlServer(connectionString));

            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
            services.AddSingleton<ISpelMovement, SpelMovementService>();

            return services;
        }
    }
}
