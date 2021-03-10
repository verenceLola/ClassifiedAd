using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Marketplace
{
    public static class AppBuilderDatabaseEntensions
    {
        public static void EnsureDatabase(this IApplicationBuilder app)
        {
            var context = app.ApplicationServices.GetService<Infrastructure.ClassifiedAdDbContext>();

            if (!context.Database.EnsureCreated())
            {
                context.Database.Migrate();
            }
        }
    }
}
