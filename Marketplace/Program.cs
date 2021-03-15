using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using static System.Reflection.Assembly;
using static System.Environment;
using Serilog;


namespace MarketPlace
{
    public class Program
    {
        static Program() => CurrentDirectory = Path.GetDirectoryName(GetEntryAssembly().Location);
        public static void Main(string[] args)
        {
            var configuration = BuildConfiguration(args);
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();
            ConfigureWebHost(configuration).Build().Run();
        }

        public static IWebHostBuilder ConfigureWebHost(IConfiguration configuration) =>
            new WebHostBuilder().UseStartup<Startup>()
            .UseConfiguration(configuration)
            .ConfigureServices(services => services.AddSingleton(configuration))
            .UseContentRoot(CurrentDirectory)
            .UseKestrel();

        private static IConfiguration BuildConfiguration(string[] args) =>
            new ConfigurationBuilder()
            .SetBasePath(CurrentDirectory)
            .AddJsonFile("appsettings.Development.json", false, true)
            .Build();
    }
}
