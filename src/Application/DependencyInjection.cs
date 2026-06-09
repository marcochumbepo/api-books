using Microsoft.Extensions.DependencyInjection;
using MsBooks.Application.Services;
using MsBooks.Application.Validators;

namespace MsBooks.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateBookService>();
        services.AddScoped<GetBooksService>();
        services.AddScoped<GetBookByIdService>();
        services.AddScoped<UpdateBookService>();
        services.AddScoped<AuthService>();

        services.AddScoped<CreateBookRequestValidator>();

        return services;
    }
}
