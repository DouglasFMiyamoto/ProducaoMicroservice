using Microsoft.EntityFrameworkCore;
using ProducaoMicroservice.Adapters.Database.PostgreSQL;

namespace ProducaoMicroservice.Adapters.Database.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using ProducaoContext dbContext =
                scope.ServiceProvider.GetRequiredService<ProducaoContext>();

            dbContext.Database.Migrate();
        }
    }
}
