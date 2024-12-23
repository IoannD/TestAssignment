using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestAssignment.DB.Repositories;
using TestAssignment.Domain.AbstractRepositories;

namespace TestAssignment.DB.Extensions;

public static class DbExtension
{
    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Context>(options =>
        {
            var connStr = configuration.GetConnectionString("DB_CONNECTION_STRING") ??
                          throw new ArgumentException("DB connection string not found");
            options.UseNpgsql(connStr, b => b.MigrationsAssembly("TestAssignment.DB"));
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        return services;
    }
}