using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Raven.Client.Documents;

using Marketplace.Api.ApplicationServices;
using MarketPlace.Domain.Interfaces;
using MarketPlace.Domain.Services.Interfaces;
using MarketPlace.Framework;


namespace Marketplace
{
    public class Startup
    {
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }
        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            var store = new DocumentStore
            {
                Urls = new[] { "http://localhost:8080" },
                Database = "MarketPlace_Chapter8",
                Conventions = {
                    FindIdentityProperty = m => m.Name == "DbId"
                },
            };
            store.Initialize();

            services.AddSingleton<IcurrencyLookup, Infrastructure.FixedCurrencyLookup>();
            services.AddScoped(c => store.OpenAsyncSession());
            services.AddScoped<IUnitOfWork, Infrastructure.RavenDbUnitOfWork>();
            services.AddScoped<IClassifiedAdRepository, Infrastructure.ClassifiedAdRepository>();
            services.AddScoped<Api.ApplicationServices.Interfaces.IApplicationService, ClassfiedAdApplicationService>();

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddSwaggerGen(c =>
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "ClassfiedAds",
                Version = "v1"
            }));
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClassfiedAds v1"));
        }
    }
}
