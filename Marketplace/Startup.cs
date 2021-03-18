using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using MarketPlace.Services.ApplicationServices;
using System.Threading.Tasks;
using MarketPlace.Domain.Services.Interfaces;
using MarketPlace.Framework;
using EventStore.ClientAPI;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;


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
            var documentStore = ConfigureRavenDB(Configuration.GetSection("ravenDB"));
            Func<IAsyncDocumentSession> getSession = () => documentStore.OpenAsyncSession();

            services.AddTransient(c => getSession());
            services.AddSingleton<IEnumerable<ReadModels.UserDetails.UserDetails>>(userDetails);
            services.AddSingleton<IEnumerable<ReadModels.ClassifiedAds.ClassfiedAdDetails>>(classfiedAdDetails);
            services.AddSingleton(esConnection);
            services.AddSingleton<IAggregateStore>(store);

            var projectionManager = new Infrastructure.ProjectionManager(esConnection,
                new Infrastructure.RavenDBCheckpointStore(getSession, "readmodels"),
                new Projections.ClassifiedAdDetailProjection(getSession,
                    async userId => (await getSession.GetUserDetails(userId))?.DisplayName),
                new Projections.UserDetailsProjection(getSession),
                new Projections.ClassifiedAdUpcasters(esConnection, async userId => (await getSession.GetUserDetails(userId))?.PhotoUrl)
            );
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
        private static IDocumentStore ConfigureRavenDB(IConfiguration configuration)
        {
            var store = new DocumentStore
            {
                Urls = new[] { configuration["server"] },
                Database = configuration["database"]
            };

            store.Conventions.RegisterAsyncIdConvention<ReadModels.ClassifiedAds.ClassfiedAdDetails>(
                (dbName, detail) => Task.FromResult(string.Format("ClassfiedAdDetails/{0}", detail.ClassifiedAdId))
            );
            store.Conventions.RegisterAsyncIdConvention<ReadModels.UserDetails.UserDetails>(
                (dbName, detail) => Task.FromResult(string.Format("UserDetail/{0}", detail.UserId))
            );

            store.Initialize();
            var record = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));
            if (record == null)
            {
                store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(store.Database)));
            }

            return store;
        }
    }
}
