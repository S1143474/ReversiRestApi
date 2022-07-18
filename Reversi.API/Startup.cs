using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
/*using Microsoft.Extensions.Logging;*/
using Reversi.API.Application;
using Reversi.API.Infrastructure;

namespace Reversi.API
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers();
            
            services.AddApplication();
            services.AddInfrastructure(Configuration);
            services.AddAutoMapper(typeof(Startup));

            /*services.AddSingleton<ISpelRepository, SpelAccessLayer>();*/

            services
                .AddMvcCore(options =>
                {
                    options.RequireHttpsPermanent = true;
                    options.RespectBrowserAcceptHeader = true;

                })
                .AddFormatterMappings()
                .AddCors(c =>
                {
                    c.AddPolicy(MyAllowSpecificOrigins, options =>
                        options.WithOrigins(new string[]
                                { "https://localhost:44309", "http://localhost:3000", "http://127.0.0.1:5500/" })
                            .WithMethods("GET", "PUT", "POST")
                            .WithHeaders("content-type")
                    );
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ExceptionMiddleware.ExceptionMiddleware>();

            app.UseHttpsRedirection();
                
            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(options =>
                options.WithOrigins("https://localhost:44309", "http://localhost:3000")
                    .WithMethods("GET", "PUT", "POST")
                    .WithHeaders("content-type")
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // TODO: mayby change the way this is setup.
            loggerFactory.AddFile("Logs/reversi-rest-api-{Date}.txt");
        }
    }
}
