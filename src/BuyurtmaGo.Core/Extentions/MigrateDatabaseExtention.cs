using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuyurtmaGo.Core.Extentions
{
    public static class MigrateDatabaseExtention
    {
        public static void MigrateDatabase(this WebApplication app)
        {
            if (app is null)
                throw new ArgumentNullException(nameof(app), "Web application is null");

            using var scope = app.Services.CreateScope();

            using var _dbContext = scope.ServiceProvider.GetRequiredService<BuyurtmaGoDb>();

            if (_dbContext is null)
                throw new ArgumentNullException(nameof(_dbContext), "Database context is null");

            if (_dbContext.Database.GetPendingMigrations().Any())
            {
                _dbContext.Database.Migrate();
            }

            return;
        }
    }
}
