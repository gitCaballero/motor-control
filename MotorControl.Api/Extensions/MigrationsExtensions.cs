using Microsoft.EntityFrameworkCore;
using MotorControl.Api.Repository.Context;

namespace MotorControl.Api.Extensions
{
    public static class MigrationsExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();

            using MotorControlDbContext dbContext = scope.ServiceProvider.GetRequiredService<MotorControlDbContext>();

            dbContext.Database.Migrate();
        }
    }
}
