using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Reversi.API.Application.Common.Behaviours;
using Reversi.API.Application.Common.Helpers;
using Reversi.API.Application.Common.Interfaces;
using Reversi.API.Domain.Entities;

namespace Reversi.API.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IRequestContext, RequestContext>();
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnHandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

            services.AddScoped<ISortHelper<Spel>, SortHelper<Spel>>();

            return services;
        }
    }
}
