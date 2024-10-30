using Dima.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api
{
    public class DatabaseInitializer : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseInitializer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var sqlFilePaths = new List<string>();
                
                sqlFilePaths.Add(Path.Combine(AppContext.BaseDirectory, "Data", "Views", "vwGetIncomesAndExpenses.sql"));
                sqlFilePaths.Add(Path.Combine(AppContext.BaseDirectory, "Data", "Views", "vwGetExpensesByCategory.sql"));
                sqlFilePaths.Add(Path.Combine(AppContext.BaseDirectory, "Data", "Views", "vwGetIncomesByCategory.sql"));

                foreach (var sqlFilePath in sqlFilePaths)
                {
                    if (File.Exists(sqlFilePath))
                    {
                        var sqlScript = await File.ReadAllTextAsync(sqlFilePath, cancellationToken);
                        await context.Database.ExecuteSqlRawAsync(sqlScript, cancellationToken);
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }

}

