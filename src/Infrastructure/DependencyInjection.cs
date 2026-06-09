using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsBooks.Domain.Interfaces;
using MsBooks.Infrastructure.Data;
using MsBooks.Infrastructure.Repositories;

namespace MsBooks.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var provider = configuration["Database:Provider"] ?? "InMemory";

        services.AddDbContext<AppDbContext>(options =>
        {
            switch (provider.ToLowerInvariant())
            {
                case "sqlserver":
                    var connectionString = configuration.GetConnectionString("DefaultConnection");
                    options.UseSqlServer(connectionString);
                    break;

                case "postgresql":
                    throw new NotSupportedException(
                        "PostgreSQL no esta implementado.");
                case "inmemory":
                default:
                    options.UseInMemoryDatabase("BooksDb");
                    break;
            }
        });

        services.AddScoped<IBookRepository, BookRepository>();

        return services;
    }
}
