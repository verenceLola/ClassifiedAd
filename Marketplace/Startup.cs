using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Microsoft.EntityFrameworkCore;
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
            const string connetionString =
                @"Host=localhost;Database=Marketplace_Chapter8;
                Username=ddd;Password=book";

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<Infrastructure.ClassifiedAdDbContext>(
                    options => options.UseNpgsql(connetionString)
                );
            services.AddSingleton<IcurrencyLookup, Infrastructure.FixedCurrencyLookup>();
            services.AddScoped<IUnitOfWork, Infrastructure.EfCoreUnitOfWork>();
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

            app.EnsureDatabase();
            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClassfiedAds v1"));
        }
    }
}
