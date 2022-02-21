using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Infrastructure.Persistence;

namespace Reversi.API.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSingleton<ISpelRepository, SpelAccessLayer>();
            return services;
        }
    }
}
