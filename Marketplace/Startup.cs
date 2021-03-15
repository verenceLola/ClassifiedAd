using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using MarketPlace.Services.ApplicationServices;
using MarketPlace.Domain.Interfaces;
using MarketPlace.Domain.Services.Interfaces;
using MarketPlace.Framework;
using EventStore.ClientAPI;


namespace MarketPlace
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
            var purgomalumClient = new Infrastructure.PurgomalumClient();

            var esConnection = EventStoreConnection.Create(
                Configuration.GetConnectionString("EventStore"),
                ConnectionSettings.Create().KeepReconnecting(),
                Environment.ApplicationName
            );

            var store = new Infrastructure.EsAggregateStore(esConnection);
            var classfiedAdDetails = new List<ReadModels.ClassifiedAds.ClassfiedAdDetails>();
            var userDetails = new List<ReadModels.UserDetails.UserDetails>();

            services.AddSingleton<IEnumerable<ReadModels.UserDetails.UserDetails>>(userDetails);
            services.AddSingleton<IEnumerable<ReadModels.ClassifiedAds.ClassfiedAdDetails>>(classfiedAdDetails);
            services.AddSingleton(esConnection);
            services.AddSingleton<IAggregateStore>(store);

            IProjection[] projections = {
                new Projections.ClassifiedAdDetailProjection(classfiedAdDetails),
                new Projections.UserDetailsProjection(userDetails)
            };

            var projectionManager = new Infrastructure.ProjectionManager(esConnection, projections);
            services.AddSingleton<IHostedService>(new EventStoreService(esConnection, projectionManager));
            services.AddSingleton<IcurrencyLookup, Infrastructure.FixedCurrencyLookup>();
            services.AddScoped<Services.ApplicationServices.Interfaces.IApplicationService, ClassfiedAdApplicationService>();
            services.AddSingleton(new UserProfileApplicationService(
                store, text => purgomalumClient.CheckForProfanity(text).GetAwaiter().GetResult()));


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
