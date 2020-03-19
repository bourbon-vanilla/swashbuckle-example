using Microsoft.Extensions.DependencyInjection;

using SwashbuckleExample.Services;


namespace SwashbuckleExample
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalServices(this IServiceCollection services)
        {
            return services
                .AddScoped<IBookRepository, BookRepository>()
                .AddScoped<IAuthorRepository, AuthorRepository>();
        }
    }
}
