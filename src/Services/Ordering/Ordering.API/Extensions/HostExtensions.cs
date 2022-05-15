using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ordering.Infrastructure.Presistense;

namespace Ordering.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase(this IHost host)
        {
            using(var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetService<ILogger<OrderContext>>();
                var context = services.GetService<OrderContext>();

                logger.LogInformation("Start migrating");
                context.Database.MigrateAsync().Wait();
                OrderContextSeed.SeedAsync(context).Wait();
                logger.LogInformation("Finish migrating");
            }
            return host;
        }
    }
}
